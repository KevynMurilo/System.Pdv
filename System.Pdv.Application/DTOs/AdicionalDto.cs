using System.ComponentModel.DataAnnotations;

namespace System.Pdv.Application.DTOs;

public class AdicionalDto
{
    [Required(ErrorMessage = "O nome do item adicional é obrigatório.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O preço do item adicional é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço item adicional deve ser maior que zero.")]
    public decimal Preco { get; set; }
}
