using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
using System.Text.Json.Serialization;

namespace System.Pdv.Application.DTOs;

public class PedidoExternoDto
{
    public Guid ClienteId { get; set; }
    public Guid GarcomId { get; set; }
    [JsonIgnore]
    public TipoPedido TipoPedido { get; set; } = TipoPedido.Externo;
    public Guid MetodoPagamentoId { get; set; }
    public Guid StatusPedidoId { get; set; }
    public IList<ItemPedidoDto> Itens { get; set; }
}
