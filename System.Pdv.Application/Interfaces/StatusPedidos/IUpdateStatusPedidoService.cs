using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.StatusPedidos;

public interface IUpdateStatusPedidoService
{
    Task<OperationResult<StatusPedido>> ExecuteAsync(Guid id, StatusPedidoDto statusPedidoDto);
}
