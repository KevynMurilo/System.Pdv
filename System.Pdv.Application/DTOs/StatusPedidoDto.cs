using System.ComponentModel.DataAnnotations;

namespace System.Pdv.Application.DTOs;

public class StatusPedidoDto
{
    [Required(ErrorMessage = "Status do pedido é obrigatório")]
    public string Status { get; set; }
}
