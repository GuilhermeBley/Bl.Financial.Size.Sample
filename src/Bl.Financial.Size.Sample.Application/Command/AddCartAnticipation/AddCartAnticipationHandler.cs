
using Bl.Financial.Size.Sample.Application.Model;
using Bl.Financial.Size.Sample.Application.Repository;

namespace Bl.Financial.Size.Sample.Application.Command.AddCartAnticipation;

public record AddCartAnticipationRequest(
    long NfId)
    : IRequest<AddCartAnticipationResponse>;

public record AddCartAnticipationResponse(long AnticipationCartItemId);

internal class AddCartAnticipationHandler
    : IRequestHandler<AddCartAnticipationRequest, AddCartAnticipationResponse>
{
    private readonly FinancialContext _financialContext;

    public AddCartAnticipationHandler(FinancialContext financialContext)
    {
        _financialContext = financialContext;
    }

    public async Task<AddCartAnticipationResponse> Handle(AddCartAnticipationRequest request, CancellationToken cancellationToken)
    {
        var nfAlreadyInserted = await _financialContext
            .Anticipations
            .AsNoTracking()
            .Where(x => x.NfId == request.NfId)
            .AnyAsync(cancellationToken);

        if (nfAlreadyInserted)
        {
            throw new CoreException(409, "NF already inserted.");
        }

        var nf = await _financialContext.
            Nfs
            .AsNoTracking()
            .Where(x => x.Id == request.NfId)
            .Select(x => new
            {
                x.Id,
                x.Value,
                x.DueDate,
                x.CompanyId
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (nf == null)
            throw new CoreException(404, "NF not found");

        var companyInfo = await _financialContext
            .Companies
            .AsNoTracking()
            .Where(x => x.Id == nf.CompanyId)
            .Select(x => new
            {
                x.MonthlyBilling,
                x.ServiceKind,
            })
            .SingleAsync(cancellationToken);

        if (companyInfo.MonthlyBilling < 10_000)
        {
            throw new CoreException(405, "The company must at least have 10,000 of monthly billing to anticipate some NF.");
        }

        var maxCartValue = GetMaxAnticipationValue(companyInfo.MonthlyBilling, companyInfo.ServiceKind);
        var anticipationDays = (DateTime.UtcNow - nf.DueDate.ToDateTime(TimeOnly.MinValue)).TotalDays * -1;
        const double tax = 0.0465;
        decimal desagio = Math.Round(
            (decimal)((double)nf.Value / Math.Pow(1 + tax, anticipationDays / 30)),
            2);

        await using var transaction =
            await _financialContext.Database.BeginTransactionAsync(cancellationToken);

        var insertionResult = await _financialContext
            .Anticipations
            .AddAsync(new Model.NfAnticipationModel()
            {
                Desagio = nf.Value - desagio,
                LiquidValue = desagio,
                TotalValue = nf.Value,
                NfId = nf.Id,
                CompanyId = nf.CompanyId,
            });

        await _financialContext.SaveChangesAsync(cancellationToken);

        var currentTotalCartValue = await _financialContext
            .Anticipations
            .AsNoTracking()
            .Where(x => x.CompanyId == nf.CompanyId)
            .SumAsync(x => x.TotalValue, cancellationToken);

        if (currentTotalCartValue > maxCartValue)
        {
            await transaction.RollbackAsync();
            throw new CoreException(400, $"The total value was exceed. Max allowed: {maxCartValue}.");
        }

        await transaction.CommitAsync();

        return new AddCartAnticipationResponse(insertionResult.Entity.Id);
    }

    private decimal GetMaxAnticipationValue(
        decimal monthlyBilling,
        CompanyServiceKind kind)
    {
        if (monthlyBilling >= 100_001 && kind == CompanyServiceKind.Service)
        {
            return monthlyBilling * 0.60m;
        }
        if (monthlyBilling >= 100_001 && kind == CompanyServiceKind.Product)
        {
            return monthlyBilling * 0.65m;
        }
        if (monthlyBilling >= 50_001 && kind == CompanyServiceKind.Service)
        {
            return monthlyBilling * 0.55m;
        }
        if (monthlyBilling >= 50_001 && kind == CompanyServiceKind.Product)
        {
            return monthlyBilling * 0.60m;
        }
        if (monthlyBilling >= 10_000)
        {
            return monthlyBilling * 0.50m;
        }

        return 0;
    }
}
