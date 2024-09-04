﻿using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.StatusPedidos;

public interface IDeleteStatusPedidoService
{
    Task<OperationResult<StatusPedido>> ExecuteAsync(Guid id);
}
