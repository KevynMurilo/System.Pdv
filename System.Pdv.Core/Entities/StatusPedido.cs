using System.Text.Json.Serialization;

namespace System.Pdv.Core.Entities;

public class StatusPedido
{
    public Guid Id { get; set; }
    public string Status { get; set; }

    [JsonIgnore]
    public ICollection<Pedido> Pedidos { get; set; }
}
