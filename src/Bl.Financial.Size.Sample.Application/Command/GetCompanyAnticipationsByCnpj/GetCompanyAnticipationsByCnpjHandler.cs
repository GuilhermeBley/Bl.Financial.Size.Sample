
using Bl.Financial.Size.Sample.Application.Model;
using Bl.Financial.Size.Sample.Application.Repository;

namespace Bl.Financial.Size.Sample.Application.Command.GetCompanyAnticipationsByCnpj;

public record GetCompanyAnticipationsByCnpjRequest(string Cnpj)
    : IRequest<GetCompanyAnticipationsByCnpjResponse>;

public record GetCompanyAnticipationsByCnpjAnticipationItemResponse(
    long NfNumber,
    decimal Total,
    decimal TotalLiquid,
    decimal Desagio);

public record GetCompanyAnticipationsByCnpjResponse(
    string CompanyName,
    string Cnpj,
    decimal Limit,
    decimal Total,
    decimal TotalLiquid,
    GetCompanyAnticipationsByCnpjAnticipationItemResponse[] Anticipations);

public class GetCompanyAnticipationsByCnpjHandler
    : IRequestHandler<GetCompanyAnticipationsByCnpjRequest, GetCompanyAnticipationsByCnpjResponse>
{
    private readonly FinancialContext _context;

    public GetCompanyAnticipationsByCnpjHandler(FinancialContext context)
    {
        _context = context;
    }

    public async Task<GetCompanyAnticipationsByCnpjResponse> Handle(
        GetCompanyAnticipationsByCnpjRequest request, 
        CancellationToken cancellationToken)
    {
        var cnpjParsed = string.Concat(request.Cnpj.Where(char.IsNumber));

        var company = await _context.Companies
            .AsNoTracking()
            .Where(x => x.Cnpj == cnpjParsed)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Cnpj,
                x.MonthlyBilling,
                x.ServiceKind
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (company is null)
            throw new CoreException(404, "Company not found.");

        var anticipations = await (
            from ant in _context.Anticipations
            join nf in _context.Nfs on ant.NfId equals nf.Id
            where ant.CompanyId == company.Id
            select new GetCompanyAnticipationsByCnpjAnticipationItemResponse(
                nf.Number,
                ant.TotalValue,
                ant.LiquidValue,
                ant.Desagio))
            .ToArrayAsync(cancellationToken);

        var limit = GetMaxAnticipationValue(company.MonthlyBilling, company.ServiceKind);

        return new(
            company.Name,
            company.Cnpj,
            limit,
            Total: anticipations.Sum(x => x.Total),
            TotalLiquid: anticipations.Sum(x => x.TotalLiquid),
            Anticipations: anticipations);
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
