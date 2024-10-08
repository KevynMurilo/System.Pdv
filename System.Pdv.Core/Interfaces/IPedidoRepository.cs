﻿using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IPedidoRepository
{
    Task<IEnumerable<Pedido>> GetPedidosAsync(int pageNumber, int pageSize, string tipoPedido, string statusPedido);
    Task<IEnumerable<Pedido>> GetPedidosByMesaAsync(int numeroMesa, string status);
    Task<Pedido> GetByIdAsync(Guid id);
    Task<List<Pedido>> GetPedidosByIdsAsync(IEnumerable<Guid> ids);
    Task<Cliente> GetClienteByNomeTelefoneAsync(string nome, string telefone);
    Task AddAsync(Pedido pedido);
    Task RemoveItem(ItemPedido item);
    Task UpdateAsync(Pedido pedido);
    Task DeleteAsync(Pedido pedido);
}
