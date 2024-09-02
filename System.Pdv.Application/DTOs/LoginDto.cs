using System.ComponentModel.DataAnnotations;

namespace System.Pdv.Application.DTOs;

public class LoginDto
{
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    [Required(ErrorMessage = "Email é obrigatório")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Senha é obrigatória")]
    public string Password { get; set; }
}
