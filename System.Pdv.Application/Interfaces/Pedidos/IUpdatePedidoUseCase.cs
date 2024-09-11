using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;
using System.Security.Claims;

namespace System.Pdv.Application.Interfaces.Pedidos;

public interface IUpdatePedidoUseCase
{
    Task<OperationResult<Pedido>> ExecuteAsync(Guid id, PedidoDto pedidoDto, ClaimsPrincipal userClaims);
}
