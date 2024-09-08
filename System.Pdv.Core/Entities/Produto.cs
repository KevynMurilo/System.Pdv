using System.Text.Json.Serialization;

namespace System.Pdv.Core.Entities;

public class Produto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public bool Disponivel { get; set; } = true;

    [JsonIgnore]
    public Guid? CategoriaId { get; set; }   
    public Categoria Categoria { get; set; }

    [JsonIgnore]
    public ICollection<ItemPedido> Itens { get; set; }
}
