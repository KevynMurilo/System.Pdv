using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Pedidos;

public interface IProcessarItensPedidoService
{
    Task<OperationResult<Pedido>> ExecuteAsync(IList<ItemPedidoDto> itensDto, Pedido pedido);
}
