﻿using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Pedidos;

public interface IUpdatePedidoInternoService
{
    Task<OperationResult<Pedido>> ExecuteAsync(Guid id, PedidoInternoDto pedidoInternoDto);
}
