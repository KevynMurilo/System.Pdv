using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IMesaRepository
{
    Task<IEnumerable<Mesa>> GetAllAsync();
    Task<Mesa> GetByIdAsync(Guid id);
    Task<Mesa> GetByNumberAsync(int numero);
    Task<Mesa> AddAsync(Mesa mesa);
    Task UpdateAsync(Mesa mesa);
    Task DeleteAsync(Mesa mesa);
}
