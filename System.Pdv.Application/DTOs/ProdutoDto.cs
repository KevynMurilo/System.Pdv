using System.ComponentModel.DataAnnotations;

namespace System.Pdv.Application.DTOs;

public class ProdutoDto
{
    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome do produto deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; }

    [StringLength(500, ErrorMessage = "A descrição do produto deve ter no máximo 500 caracteres.")]
    public string Descricao { get; set; }

    [Required(ErrorMessage = "O preço do produto é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço do produto deve ser maior que zero.")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "A disponibilidade do produto é obrigatória.")]
    public bool Disponivel { get; set; }

    [Required(ErrorMessage = "O ID da categoria é obrigatório.")]
    public Guid CategoriaId { get; set; }
}
