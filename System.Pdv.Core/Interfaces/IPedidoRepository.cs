using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IPedidoRepository
{
    Task<IEnumerable<Pedido>> GetPedidosAsync(int pageNumber, int pageSize, string type, string status);
    Task<Pedido> GetByIdAsync(Guid id);
    Task AddAsync(Pedido pedido);
    Task RemoveItem(ItemPedido item);
    Task UpdateAsync(Pedido pedido);
    Task DeleteAsync(Pedido pedido);
}
