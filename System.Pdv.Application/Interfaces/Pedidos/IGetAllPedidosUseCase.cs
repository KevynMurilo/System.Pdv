﻿using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Pedidos;

public interface IGetAllPedidosUseCase
{
    Task<OperationResult<IEnumerable<Pedido>>> ExecuteAsync(int pageNumber, int pageSize, string type, string status);
}
