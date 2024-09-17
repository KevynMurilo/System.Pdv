using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IPermissaoRepository
{
    Task<Permissao> GetByIdAsync(Guid permissaoId);
    Task<ICollection<Permissao>> GetAllPermissao(int pageNumber, int pageSize, string recurso, string acao);
    Task<ICollection<Permissao>> GetAllPermissionWithRoleAsync(int pageNumber, int pageSize, string recurso, string acao);
    Task<ICollection<Permissao>> GetAllPermissaoByRoleIdAsync(Guid roleId);
    Task AddAsync(Permissao permissao);
    Task UpdateAsync(Permissao permissao);
    Task DeleteAsync(Permissao permissao);
}