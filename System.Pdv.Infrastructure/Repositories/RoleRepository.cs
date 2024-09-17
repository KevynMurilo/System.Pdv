using Microsoft.EntityFrameworkCore;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Data;

namespace System.Pdv.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;
    public RoleRepository(AppDbContext context)
    { 
        _context = context;
    }
    public async Task<IEnumerable<Role>> GetAllRolesAsync()
    {
        return await _context.Roles.AsNoTracking().ToListAsync();
    }

    public async Task<Role?> GetByIdAsync(Guid id)
    {
        return await _context.Roles
            .Include(p => p.Permissoes)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task UpdateAsync(Role role)
    {
        _context.Roles.Update(role);
        await _context.SaveChangesAsync();
    }
}
