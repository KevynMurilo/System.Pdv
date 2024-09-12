using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Pedidos;

public interface IPrintPedidoByIdsUseCase
{
    Task<OperationResult<List<Pedido>>> ExecuteAsync(IEnumerable<Guid> ids);
}
