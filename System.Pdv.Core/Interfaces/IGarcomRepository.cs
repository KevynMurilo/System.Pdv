using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IGarcomRepository
{
    Task<IEnumerable<Garcom>> GetAllAsync();
    Task<Garcom> GetByIdAsync(Guid id);
    Task AddAsync(Garcom garcom);
}
