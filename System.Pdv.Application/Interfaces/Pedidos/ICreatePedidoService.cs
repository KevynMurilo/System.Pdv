using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Pedidos;

public interface ICreatePedidoService
{
    Task<OperationResult<Pedido>> ExecuteAsync(PedidoDto pedidoDto);
}
