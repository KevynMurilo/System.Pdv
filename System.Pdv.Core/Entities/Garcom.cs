using System.Text.Json.Serialization;

namespace System.Pdv.Core.Entities;

public class Garcom
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }

    [JsonIgnore]
    public string PasswordHash { get; set; }

    [JsonIgnore]
    public ICollection<Pedido> Pedidos { get; set; }
}
