using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IAdicionalRepository
{
    Task<IEnumerable<ItemAdicional>> GetAllAsync(int pageNumber, int pageSize);
    Task<ItemAdicional> GetByIdAsync(Guid id);
    void AttachAdicionais(IEnumerable<ItemAdicional> adicionais);
    Task<ItemAdicional> GetByNameAsync(string nome);
    Task<List<ItemAdicional>> GetAdicionaisByIdsAsync(List<Guid> ids);
    Task AddAsync(ItemAdicional itemAdicional);
    Task UpdateAsync(ItemAdicional itemAdicional);
    Task DeleteAsync(ItemAdicional itemAdicional);
}
