namespace System.Pdv.Core.Entities;

public class ItemAdicional
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }

    public ICollection<ItemPedido> Itens { get; set; }
}
