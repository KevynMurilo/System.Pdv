using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> GetAllAsync(int pageNumber, int pageSize);
    Task<Usuario> GetByIdAsync(Guid id);
    Task<Usuario> GetByEmail(string email);
    Task AddAsync(Usuario usuario);
    Task UpdateAsync(Usuario usuario);
    Task DeleteAsync(Usuario usuario);
}
