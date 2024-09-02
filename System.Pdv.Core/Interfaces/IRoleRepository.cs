using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAllRolesAsync();
    Task<Role> GetByIdAsync(Guid id);
}
