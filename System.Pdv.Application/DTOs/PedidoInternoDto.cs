using System.ComponentModel.DataAnnotations;
using System.Pdv.Core.Enums;
using System.Text.Json.Serialization;

namespace System.Pdv.Application.DTOs;

public class PedidoInternoDto
{
    [Required(ErrorMessage = "O ID da mesa é obrigatório.")]
    public Guid MesaId { get; set; }

    [Required(ErrorMessage = "O ID do garçom é obrigatório.")]
    public Guid GarcomId { get; set; }

    [JsonIgnore]
    public TipoPedido TipoPedido { get; set; } = TipoPedido.Interno;

    [Required(ErrorMessage = "O ID do método de pagamento é obrigatório.")]
    public Guid MetodoPagamentoId { get; set; }

    [Required(ErrorMessage = "O ID do status do pedido é obrigatório.")]
    public Guid StatusPedidoId { get; set; }

    [Required(ErrorMessage = "Os itens do pedido são obrigatórios.")]
    [MinLength(1, ErrorMessage = "O pedido deve conter pelo menos um item.")]
    public IList<ItemPedidoDto> Itens { get; set; }
}
