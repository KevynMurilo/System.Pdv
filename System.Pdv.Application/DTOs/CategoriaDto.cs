using System.ComponentModel.DataAnnotations;

namespace System.Pdv.Application.DTOs;

public class CategoriaDto
{
    [Required(ErrorMessage = "Nome da categoria é obrigatório")]
    public string Nome { get; set; }
}
