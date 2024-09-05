using System.Pdv.Core.Enums;
using System.Text.Json.Serialization;

namespace System.Pdv.Core.Entities;

public class Pedido
{
    public Guid Id { get; set; }

    public Guid? MesaId { get; set; }
    [JsonIgnore]
    public Mesa Mesa { get; set; }

    public Guid? ClienteId { get; set; }
    [JsonIgnore]
    public Cliente Cliente { get; set; }

    public Guid GarcomId { get; set; }
    [JsonIgnore]
    public Usuario Garcom { get; set; }

    public TipoPedido TipoPedido { get; set; } = TipoPedido.Externo;

    public Guid MetodoPagamentoId { get; set; }
    [JsonIgnore]
    public MetodoPagamento MetodoPagamento { get; set; }

    public Guid StatusPedidoId { get; set; }
    [JsonIgnore]
    public StatusPedido StatusPedido { get; set; }

    public ICollection<ItemPedido> Items { get; set; }
}
