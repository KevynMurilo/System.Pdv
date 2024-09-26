using System.ComponentModel.DataAnnotations;

namespace System.Pdv.Application.DTOs;

public class CreateRoleDto
{
    [Required(ErrorMessage = "Nome de role é obrigatório")]
    public string Nome { get; set; }
    public string Descricao { get; set; }
}
