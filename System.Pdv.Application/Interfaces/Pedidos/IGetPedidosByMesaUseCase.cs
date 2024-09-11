using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Pedidos;

public interface IGetPedidosByMesaUseCase
{
    Task<OperationResult<IEnumerable<Pedido>>> ExecuteAsync(int numeroMesa, string status);
}
