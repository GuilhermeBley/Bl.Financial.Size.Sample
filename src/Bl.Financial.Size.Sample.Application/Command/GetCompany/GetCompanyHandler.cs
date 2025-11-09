
using Bl.Financial.Size.Sample.Application.Repository;

namespace Bl.Financial.Size.Sample.Application.Command.GetCompany;

public record GetCompanyRequest(
    long? Id = null)
    : IRequest<GetCompanyResponse>;

public record GetCompanyResponse(GetCompanyItemResponse[] Results);

public record GetCompanyItemResponse(
    long Id,
    string CompanyName,
    string Cnpj,
    decimal MonthlyBilling);

public class GetCompanyHandler : IRequestHandler<GetCompanyRequest, GetCompanyResponse>
{
    private readonly FinancialContext _context;

    public GetCompanyHandler(FinancialContext context)
    {
        _context = context;
    }

    public async Task<GetCompanyResponse> Handle(GetCompanyRequest request, CancellationToken cancellationToken)
    {
        GetCompanyItemResponse[] result;
        if (request.Id is not null)
        {
            result = await _context
                .Companies
                .AsNoTracking()
                .Where(e => e.Id == request.Id)
                .Select(x => new GetCompanyItemResponse(
                    x.Id,
                    x.Name,
                    x.Cnpj,
                    x.MonthlyBilling))
                .ToArrayAsync(cancellationToken);
        }
        else
        {
            result = await _context
                .Companies
                .AsNoTracking()
                .Select(x => new GetCompanyItemResponse(
                    x.Id,
                    x.Name,
                    x.Cnpj,
                    x.MonthlyBilling))
                .ToArrayAsync(cancellationToken);
        }

        return new GetCompanyResponse(result);
    }
}
