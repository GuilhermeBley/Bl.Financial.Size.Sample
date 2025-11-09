
using Bl.Financial.Size.Sample.Application.Repository;

namespace Bl.Financial.Size.Sample.Application.Command.GetNf;

public record GetNfRequest(
    long? Id = null)
    : IRequest<GetNfResponse>;

public record GetNfResponse(GetNfItemResponse[] Results);

public record GetNfItemResponse(
    long Id,
    long CompanyId,
    long Number,
    decimal Value,
    DateOnly DueDate);

public class GetNfHandler : IRequestHandler<GetNfRequest, GetNfResponse>
{
    private readonly FinancialContext _context;

    public GetNfHandler(FinancialContext context)
    {
        _context = context;
    }

    public async Task<GetNfResponse> Handle(GetNfRequest request, CancellationToken cancellationToken)
    {

        GetNfItemResponse[] result;
        if (request.Id is not null)
        {
            result = await _context
                .Nfs
                .AsNoTracking()
                .Where(e => e.Id == request.Id)
                .Select(x => new GetNfItemResponse(
                    x.Id,
                    x.CompanyId,
                    x.Number,
                    x.Value,
                    x.DueDate))
                .ToArrayAsync(cancellationToken);
        }
        else
        {
            result = await _context
                .Nfs
                .AsNoTracking()
                .Select(x => new GetNfItemResponse(
                    x.Id,
                    x.CompanyId,
                    x.Number,
                    x.Value,
                    x.DueDate))
                .ToArrayAsync(cancellationToken);
        }

        return new GetNfResponse(result);
    }
}
