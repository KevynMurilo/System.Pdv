using System.Text.Json.Serialization;

namespace System.Pdv.Core.Entities;

public class Cliente
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }

    [JsonIgnore]
    public ICollection<Pedido> Pedidos { get; set; }
}
