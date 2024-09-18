using System.ComponentModel.DataAnnotations;

namespace System.Pdv.Application.DTOs;

public class RoleDto
{
    [Required(ErrorMessage = "Nome de role é obrigatório")]
    public string Nome { get; set; }
    public string Descricao { get; set; }
}
