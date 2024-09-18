using System.Text.Json.Serialization;

namespace System.Pdv.Core.Entities;

public class Permissao
{
    public Guid Id { get; set; }
    public string Recurso { get; set; }  // O recurso (por exemplo, "Produto")
    public string Acao { get; set; }     // A ação (por exemplo, "Criar", "Atualizar", "Deletar")

    public ICollection<Role> Roles { get; set; }
}
