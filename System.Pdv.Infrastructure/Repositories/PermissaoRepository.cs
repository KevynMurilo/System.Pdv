using Microsoft.EntityFrameworkCore;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Data;

namespace System.Pdv.Infrastructure.Repositories;

public class PermissaoRepository : IPermissaoRepository
{
    private readonly AppDbContext _context;

    public PermissaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Permissao?> GetByIdAsync(Guid permissaoId)
    {
        return await _context.Permissoes
            .Include(r => r.Roles)
            .FirstOrDefaultAsync(p => p.Id == permissaoId);
    }

    public async Task<ICollection<Permissao>> GetAllAsync()
    {
        return await _context.Permissoes
            .AsNoTracking()
            .Include(r => r.Roles)
            .ToListAsync();
    }

    public async Task AddAsync(Permissao permissao)
    {
        _context.Permissoes.Add(permissao);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Permissao permissao)
    {
        _context.Permissoes.Update(permissao);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Permissao permissao)
    {
        _context.Permissoes.Remove(permissao);
        await _context.SaveChangesAsync();
    }
}
