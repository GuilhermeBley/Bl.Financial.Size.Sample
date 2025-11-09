using System.ComponentModel.DataAnnotations;

namespace Bl.Financial.Size.Sample.Server.Model;

public class CreateAnticipationModel
{

    [Required(ErrorMessage = "Número da NF é obrigatório.")]
    public long NfId { get; set; }
}
