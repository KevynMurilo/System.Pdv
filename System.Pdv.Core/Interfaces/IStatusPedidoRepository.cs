using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IStatusPedidoRepository
{
    Task<IEnumerable<StatusPedido>> GetAllAsync();
    Task<StatusPedido> GetByIdAsync(Guid id);
    Task<StatusPedido> GetByNameAsync(string status);
    Task AddAsync(StatusPedido statusPedido);
    Task UpdateAsync(StatusPedido statusPedido);
    Task DeleteAsync(StatusPedido statusPedido);
}
