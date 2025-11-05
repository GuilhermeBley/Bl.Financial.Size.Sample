namespace Bl.Financial.Size.Sample.Application.Model;

public enum CompanyServiceKind
{
    Service,
    Product
}

public class CompanyModel
{
    public long Id { get; set; }
    public string Cnpj { get; set; } = string.Empty;
    public string NormalizedCnpj { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal MonthlyBilling { get; set; }
    public CompanyServiceKind ServiceKind { get; set; }
}
