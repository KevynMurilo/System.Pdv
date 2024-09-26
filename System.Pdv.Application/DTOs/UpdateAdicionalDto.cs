using System.ComponentModel.DataAnnotations;

namespace System.Pdv.Application.DTOs;

public class UpdateAdicionalDto
{
    public string Nome { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "O preço item adicional deve ser maior que zero.")]
    public decimal? Preco { get; set; }
}
