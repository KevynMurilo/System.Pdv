using System.ComponentModel.DataAnnotations;

namespace System.Pdv.Application.DTOs;

public class UpdateUsuarioDto
{
    public string Nome { get; set; }
    public string Email { get; set; }

    [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres")]
    public string Password { get; set; }

    public Guid RoleId { get; set; }
}
