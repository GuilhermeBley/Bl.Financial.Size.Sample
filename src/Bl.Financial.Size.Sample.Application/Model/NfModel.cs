namespace Bl.Financial.Size.Sample.Application.Model;

public class NfModel
{
    public long Id { get; set; }
    public long CompanyId { get; set; }
    public long Number { get; set; }
    public decimal Value { get; set; }
    public string UniqueId { get; set; } = string.Empty;
    public DateOnly DueDate { get; set; }
}
