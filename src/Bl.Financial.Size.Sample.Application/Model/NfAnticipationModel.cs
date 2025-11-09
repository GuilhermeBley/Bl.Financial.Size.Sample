namespace Bl.Financial.Size.Sample.Application.Model;

public class NfAnticipationModel
{
    public long Id { get; set; }
    public long NfId { get; set; }
    public long CompanyId { get; set; }
    public decimal Desagio { get; set; }
    public decimal LiquidValue { get; set; }
    /// <summary>
    /// 'Valor Bruto'
    /// </summary>
    public decimal TotalValue { get; set; }
}
