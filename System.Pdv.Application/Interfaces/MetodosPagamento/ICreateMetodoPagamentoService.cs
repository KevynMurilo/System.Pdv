﻿using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.MetodosPagamento;

public interface ICreateMetodoPagamentoService
{
    Task<OperationResult<MetodoPagamento>> ExecuteAsync(MetodoPagamentoDto metodoPagamentoDto);
}
