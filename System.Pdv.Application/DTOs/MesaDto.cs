using System.ComponentModel.DataAnnotations;
using System.Pdv.Core.Enums;

namespace System.Pdv.Application.DTOs;

public class MesaDto
{
    [Required(ErrorMessage = "Número da mesa é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "Número da mesa tem que ser maior que 0")]
    public int Numero { get; set; }

    [Required(ErrorMessage = "Status da mesa é obrigatório")]
    [EnumDataType(typeof(StatusMesa), ErrorMessage = "Status da mesa deve ser um valor válido de StatusMesa")]
    public StatusMesa Status { get; set; } = StatusMesa.Livre;
}
