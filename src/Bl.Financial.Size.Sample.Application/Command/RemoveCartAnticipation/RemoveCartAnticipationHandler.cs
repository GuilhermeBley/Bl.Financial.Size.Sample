using Bl.Financial.Size.Sample.Application.Repository;

namespace Bl.Financial.Size.Sample.Application.Command.RemoveCartAnticipation;

public record RemoveCartAnticipationRequest(
    long NfId)
    : IRequest<RemoveCartAnticipationResponse>;

public record RemoveCartAnticipationResponse();

public class RemoveCartAnticipationHandler : IRequestHandler<RemoveCartAnticipationRequest, RemoveCartAnticipationResponse>
{
    private readonly FinancialContext _financialContext;

    public RemoveCartAnticipationHandler(FinancialContext financialContext)
    {
        _financialContext = financialContext;
    }

    public async Task<RemoveCartAnticipationResponse> Handle(RemoveCartAnticipationRequest request, CancellationToken cancellationToken)
    {
        var nfAlreadyInserted = await _financialContext
            .Anticipations
            .AsTracking()
            .Where(x => x.NfId == request.NfId)
            .FirstOrDefaultAsync(cancellationToken);

        if (nfAlreadyInserted is null)
        {
            throw new CoreException(404, "Anticipation not found.");
        }

        _financialContext.Anticipations.Remove(nfAlreadyInserted);
        await _financialContext.SaveChangesAsync();

        return new RemoveCartAnticipationResponse();
    }
}
