using System.ComponentModel.DataAnnotations;

namespace Bl.Financial.Size.Sample.Server.Model;

public class CreateCompanyModel
{
    [Required(ErrorMessage = "Nome da empresa é obrigatório."),
        StringLength(500, MinimumLength = 1, ErrorMessage = "Nome da empresa inválido.")]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "CNPJ da empresa é obrigatório."),
        StringLength(20, MinimumLength = 14, ErrorMessage = "CNPJ da empresa inválido.")]
    public string Cnpj { get; set; } = string.Empty;
    [Required(ErrorMessage = "Serviço da empresa é obrigatório."),
        AllowedValues("Product", "Service", ErrorMessage = "Tipo do serviço deve ser 'Product' ou 'Service'.")]
    public string ServiceKind { get; set; } = string.Empty;
    [Required(ErrorMessage = "Faturamento Mensal da empresa é obrigatório."),
        Range(1, double.MaxValue, ErrorMessage = "Valor deve ser maior do que 1.")]
    public decimal MonthlyBilling { get; set; }
}
