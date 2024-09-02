using System.ComponentModel.DataAnnotations;

namespace System.Pdv.Application.DTOs;

public class UsuarioDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Forma de email inválido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Senha é obrigatória")]
    [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Id da role é obrigatória")]
    public Guid RoleId { get; set; }
}
