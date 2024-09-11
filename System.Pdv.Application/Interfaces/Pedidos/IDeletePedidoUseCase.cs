using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Pedidos;

public interface IDeletePedidoUseCase
{
    Task<OperationResult<Pedido>> ExecuteAsync(Guid id);
}
