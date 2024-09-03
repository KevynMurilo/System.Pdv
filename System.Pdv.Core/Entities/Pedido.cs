using System.Pdv.Core.Enums;

namespace System.Pdv.Core.Entities;

public class Pedido
{
    public Guid Id { get; set; }

    public Guid? MesaId { get; set; }
    public Mesa Mesa { get; set; }

    public Guid? ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    public Guid GarcomId { get; set; }
    public Usuario Garcom { get; set; }

    public TipoPedido TipoPedido { get; set; } = TipoPedido.Externo;

    public Guid MetodoPagamentoId { get; set; }
    public MetodoPagamento MetodoPagamento { get; set; }

    public Guid StatusPedidoId { get; set; }
    public StatusPedido StatusPedido { get; set; }

    public ICollection<ItemPedido> Items { get; set; }
}
