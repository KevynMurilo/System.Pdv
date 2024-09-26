﻿using System.ComponentModel.DataAnnotations;
using System.Pdv.Core.Enums;

namespace System.Pdv.Application.DTOs;

public class UpdateMesaDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Número da mesa tem que ser maior que 0")]
    public int? Numero { get; set; }

    [EnumDataType(typeof(StatusMesa), ErrorMessage = "Status da mesa deve ser um valor válido de StatusMesa")]
    public StatusMesa Status { get; set; } = StatusMesa.Livre;
}
