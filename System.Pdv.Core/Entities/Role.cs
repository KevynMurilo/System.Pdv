using System.Text.Json.Serialization;

namespace System.Pdv.Core.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }

    [JsonIgnore]
    public ICollection<Usuario> Usuarios { get; set; }

    [JsonIgnore]
    public ICollection<Permissao> Permissoes { get; set; }
}
