using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IPermissaoRepository
{
    Task<Permissao> GetByIdAsync(Guid permissaoId);
    Task<ICollection<Permissao>> GetAllAsync();
    Task AddAsync(Permissao permissao);
    Task UpdateAsync(Permissao permissao);
    Task DeleteAsync(Permissao permissao);
}