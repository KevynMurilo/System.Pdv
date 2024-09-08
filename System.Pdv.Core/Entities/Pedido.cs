using System.Pdv.Core.Enums;
using System.Text.Json.Serialization;

namespace System.Pdv.Core.Entities;

public class Pedido
{
    public Guid Id { get; set; }

    public Guid? MesaId { get; set; }
    [JsonIgnore]
    public Mesa Mesa { get; set; }

    [JsonIgnore]
    public Guid? ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    [JsonIgnore]
    public Guid GarcomId { get; set; }
    public Usuario Garcom { get; set; }

    public TipoPedido TipoPedido { get; set; } = TipoPedido.Externo;

    [JsonIgnore]
    public Guid MetodoPagamentoId { get; set; }
    public MetodoPagamento MetodoPagamento { get; set; }

    [JsonIgnore]
    public Guid StatusPedidoId { get; set; }
    public StatusPedido StatusPedido { get; set; }
    public DateTime DataHora { get; set; } = DateTime.UtcNow;

    public ICollection<ItemPedido> Items { get; set; }
}
