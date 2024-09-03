using System.ComponentModel.DataAnnotations;

namespace System.Pdv.Application.DTOs;

public class MetodoPagamentoDto
{
    [Required(ErrorMessage = "Nome do método de pagamento é obrigatório")]
    public string Nome { get; set; }
}
