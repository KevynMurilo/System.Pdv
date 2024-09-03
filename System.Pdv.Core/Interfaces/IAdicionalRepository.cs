using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IAdicionalRepository
{
    Task<IEnumerable<ItemAdicional>> GetAllAsync(int pageNumber, int pageSize);
    Task<ItemAdicional> GetByIdAsync(Guid id);
    Task<ItemAdicional> GetByNameAsync(string nome);
    Task AddAsync(ItemAdicional itemAdicional);
    Task UpdateAsync(ItemAdicional itemAdicional);
    Task DeleteAsync(ItemAdicional itemAdicional);
}
