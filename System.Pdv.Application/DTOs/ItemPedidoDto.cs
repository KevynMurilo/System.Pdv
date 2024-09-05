namespace System.Pdv.Application.DTOs;

public class ItemPedidoDto
{
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public string Observacoes { get; set; }
    public List<Guid> AdicionalId { get; set; }
}
