using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> GetAllAsync();
    Task<Categoria> GetByNameAsync(string nome);
    Task<Categoria> GetByIdAsync(Guid id);
    Task AddAsync(Categoria categoria);
    Task UpdateAsync(Categoria categoria);
    Task DeleteAsync(Categoria categoria);
}
