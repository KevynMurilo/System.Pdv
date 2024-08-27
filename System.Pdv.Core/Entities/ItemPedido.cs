namespace System.Pdv.Core.Entities;

public class ItemPedido
{
    public Guid Id { get; set; }

    public Guid PedidoId { get; set; }
    public Pedido Pedido { get; set; }

    public Guid ProdutoId { get; set; }
    public Produto Produto { get; set; }

    public int Quantidade { get; set; }
    public string Observacoes { get; set; }

    public List<ItemAdicional> Adicionais { get; set; }
}
