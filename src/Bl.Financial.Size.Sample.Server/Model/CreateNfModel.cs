using System.ComponentModel.DataAnnotations;

namespace Bl.Financial.Size.Sample.Server.Model;

public class CreateNfModel
{
    [Required(ErrorMessage = "Número da NF é obrigatório."),
        Range(1, double.MaxValue, ErrorMessage = "Número da NF deve ser maior do que 1.")]
    public long Number { get; set; }
    [Required(ErrorMessage = "Id da empresa é obrigatório."),
        Range(1, double.MaxValue, ErrorMessage = "Id da empresa deve ser maior do que 1.")]
    public long CompanyId { get; set; }
    [Required(ErrorMessage = "Valor é obrigatório."),
        Range(1, double.MaxValue, ErrorMessage = "Valor deve ser maior do que 1.")]
    public decimal Value { get; set; }
    public DateOnly DueDate { get; set; }

}
