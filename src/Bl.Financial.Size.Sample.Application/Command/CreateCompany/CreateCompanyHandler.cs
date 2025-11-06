
using Bl.Financial.Size.Sample.Application.Model;
using Bl.Financial.Size.Sample.Application.Repository;

namespace Bl.Financial.Size.Sample.Application.Command.CreateCompany;
public record CreateCompanyRequest(
    string CompanyName,
    string Cnpj,
    string ServiceKind,
    decimal MonthlyBilling)
    : IRequest<CreateCompanyResponse>;

public record CreateCompanyResponse(
    long Id);

public class CreateCompanyHandler : IRequestHandler<CreateCompanyRequest, CreateCompanyResponse>
{
    private readonly FinancialContext _context;
    private readonly ILogger<CreateCompanyHandler> _logger;

    public CreateCompanyHandler(FinancialContext context, ILogger<CreateCompanyHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CreateCompanyResponse> Handle(CreateCompanyRequest request, CancellationToken cancellationToken)
    {
        request = request with
        {
            CompanyName = request.CompanyName?.Trim(' ', '\n', '\t') ?? string.Empty,
            Cnpj = request.Cnpj is null ? string.Empty : string.Concat(request.Cnpj.Where(char.IsNumber)) // getting just numbers
        };

        if (string.IsNullOrWhiteSpace(request.CompanyName))
        {
            throw new CoreException("Invalid CompanyName.");
        }

        if (request.MonthlyBilling < 0)
        {
            throw new CoreException("Invalid MonthlyBilling.");
        }

        if (request.Cnpj.Length != 14)
        {
            throw new CoreException("Invalid CNPJ, should have 14 numbers.");
        }

        if (Enum.TryParse<CompanyServiceKind>(request.ServiceKind, ignoreCase: true, out var serviceKind) is false)
        {
            throw new CoreException("Invalid ServiceKind, should be 'Service' or 'Product'.");
        }

        var hasCompany = await _context
            .Companies
            .AsNoTracking()
            .Where(x => x.Cnpj == request.Cnpj)
            .AnyAsync(cancellationToken);

        if (hasCompany)
        {
            throw new CoreException(409, "Company CNPJ already exists.");
        }

        var insertionResult = await _context.Companies
            .AddAsync(new()
            {
                Name = request.CompanyName,
                MonthlyBilling = request.MonthlyBilling,
                ServiceKind = serviceKind,
                Cnpj = request.Cnpj,
            }, cancellationToken);

        await _context.SaveChangesAsync();

        return new(insertionResult.Entity.Id);
    }
}
