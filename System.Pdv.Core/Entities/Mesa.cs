using System.Pdv.Core.Enums;
using System.Text.Json.Serialization;

namespace System.Pdv.Core.Entities;

public class Mesa
{
    public Guid Id { get; set; }
    public int Numero { get; set; }
    public StatusMesa Status { get; set; } = StatusMesa.Livre;

    [JsonIgnore]
    public ICollection<Pedido> Pedidos { get; set; }
}
