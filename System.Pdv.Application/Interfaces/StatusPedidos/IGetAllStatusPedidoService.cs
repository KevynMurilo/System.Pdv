using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.StatusPedidos;

public interface IGetAllStatusPedidoService
{
    Task<OperationResult<IEnumerable<StatusPedido>>> ExecuteAsync();
}
