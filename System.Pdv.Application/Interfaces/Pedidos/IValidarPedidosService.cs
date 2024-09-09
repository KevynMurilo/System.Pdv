using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Pedidos;

public interface IValidarPedidosService
{
    Task<OperationResult<Pedido>> ValidarPedido(PedidoDto pedidoDto, string garcomId);
}
