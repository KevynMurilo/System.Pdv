using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Pedidos;

public interface IValidarPedidosService
{
    Task<OperationResult<Pedido>> ValidarPedidoInternoAsync(PedidoInternoDto pedidoDto);
    Task<OperationResult<Pedido>> ValidarPedidoExternoAsync(PedidoExternoDto pedidoExternoDto);
}
