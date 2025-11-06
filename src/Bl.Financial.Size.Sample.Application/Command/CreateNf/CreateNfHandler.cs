
using Bl.Financial.Size.Sample.Application.Repository;
using Bl.Financial.Size.Sample.Application.ValueObject;

namespace Bl.Financial.Size.Sample.Application.Command.CreateNf;

public record CreateNfRequest(
    long Number,
    long CompanyId,
    decimal Value,
    DateOnly DueDate)
    : IRequest<CreateNfResponse>;

public record CreateNfResponse(
    long Id);

public class CreateNfHandler : IRequestHandler<CreateNfRequest, CreateNfResponse>
{
    private readonly FinancialContext _context;
    private readonly ILogger<CreateNfHandler> _logger;

    public CreateNfHandler(FinancialContext context, ILogger<CreateNfHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CreateNfResponse> Handle(CreateNfRequest request, CancellationToken cancellationToken)
    {
        if (request.Number < 1)
        {
            throw new CoreException("Invalid NF number.");
        }
        if (request.Value < 1)
        {
            throw new CoreException("Invalid NF value.");
        }

        var uniqueId = $"{request.Number}-{request.CompanyId}";

        var isDuplicatedNumber = await _context
            .Nfs
            .AsNoTracking()
            .Where(x => x.UniqueId == uniqueId)
            .AnyAsync(cancellationToken);

        if (isDuplicatedNumber)
        {
            throw new CoreException(409, "NF number already exists.");
        }

        var doesCompanyExist = await _context
            .Companies
            .AsNoTracking()
            .Where(x => x.Id == request.CompanyId)
            .AnyAsync(cancellationToken);

        if (doesCompanyExist is false)
        {
            throw new CoreException(404, $"Company with ID {request.CompanyId} doesn't exist.");
        }

        var insertionResult = await _context.Nfs.AddAsync(
            new()
            {
                CompanyId = request.CompanyId,
                DueDate = request.DueDate,
                Number = request.Number,
                UniqueId = uniqueId,
                Value = request.Value,
            },
            cancellationToken);

        await _context.SaveChangesAsync();

        _logger.LogInformation("NF {0} created successfully.", insertionResult.Entity.Id);

        return new(insertionResult.Entity.Id);
    }
}
