using System.Text.Json.Serialization;

namespace System.Pdv.Core.Entities;

public class Categoria
{
    public Guid Id { get; set; }
    public string Nome { get; set; }

    [JsonIgnore]
    public ICollection<Produto> Produtos { get; set; }
}
