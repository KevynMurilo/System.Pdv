using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IProdutoRepository
{
    Task<IEnumerable<Produto>> GetAllAsync(int pageNumber, int pageSize);
    Task<Produto> GetByIdAsync(Guid id);
    Task<IEnumerable<Produto>> GetProdutoByCategoria(Guid categoriaId, int pageNumber, int pageSize);
    Task AddAsync(Produto produto);
    Task UpdateAsync(Produto produto);
    Task DeleteAsync(Produto produto);
}
