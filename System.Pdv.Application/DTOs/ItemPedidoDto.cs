using System.Text.Json.Serialization;

namespace System.Pdv.Application.DTOs;

public class ItemPedidoDto
{
    [JsonIgnore]
    public Guid? Id { get; set; }
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public string Observacoes { get; set; }
    public List<Guid> AdicionalId { get; set; }
}
