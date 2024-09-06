using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IPedidoRepository
{
    Task<IEnumerable<Pedido>> GetPedidosAsync(int pageNumber, int pageSize, string type);
    Task<Pedido> GetByIdAsync(Guid id);
    Task<Pedido> AddAsync(Pedido pedido);
    Task UpdateAsync(Pedido pedido);
    Task DeleteAsync(Pedido pedido);
}
