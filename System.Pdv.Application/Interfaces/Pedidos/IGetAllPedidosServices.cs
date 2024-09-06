using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Pedidos;

public interface IGetAllPedidosServices
{
    Task<OperationResult<IEnumerable<Pedido>>> ExecuteAsync(int pageNumber, int pageSize, string type);
}
