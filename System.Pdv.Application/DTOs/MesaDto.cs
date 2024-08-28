using System.ComponentModel.DataAnnotations;
using System.Pdv.Core.Enums;
using System.Text.Json.Serialization;

namespace System.Pdv.Application.DTOs;

public class MesaDto
{
    [Required(ErrorMessage = "Número da mesa é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "Número da mesa tem que ser maior que 0")]
    public int Numero { get; set; }
    public StatusMesa Status { get; set; } = StatusMesa.Livre;
}
