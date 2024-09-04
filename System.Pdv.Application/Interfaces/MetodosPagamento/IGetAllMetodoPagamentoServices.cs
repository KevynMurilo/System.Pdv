﻿using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.MetodosPagamento;

public interface IGetAllMetodoPagamentoServices
{
    Task<OperationResult<IEnumerable<MetodoPagamento>>> ExecuteAsync();
}
