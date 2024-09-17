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

    public async Task<ICollection<Permissao>> GetAllPermissao(int pageNumber, int pageSize, string recurso, string acao)
    {
        IQueryable<Permissao> query = _context.Permissoes
            .AsNoTracking()
            .OrderBy(p => p.Recurso)
            .ThenBy(p => p.Acao);

        if (!string.IsNullOrEmpty(recurso))
        {
            query = query.Where(p => p.Recurso.ToLower() == recurso.ToLower());
        }

        if (!string.IsNullOrEmpty(acao))
        {
            query = query.Where(p => p.Acao.ToLower() == acao.ToLower());
        }

        var pagedResult = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return pagedResult;
    }

    public async Task<ICollection<Permissao>> GetAllPermissionWithRoleAsync(int pageNumber, int pageSize, string recurso, string acao)
    {
        IQueryable<Permissao> query = _context.Permissoes
            .AsNoTracking()
            .Include(p => p.Roles)
            .OrderBy(p => p.Recurso)
            .ThenBy(p => p.Acao);

        if (!string.IsNullOrEmpty(recurso))
        {
            query = query.Where(p => p.Recurso.ToLower() == recurso.ToLower());
        }

        if (!string.IsNullOrEmpty(acao))
        {
            query = query.Where(p => p.Acao.ToLower() == acao.ToLower());
        }

        var pagedResult = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return pagedResult;
    }

    public async Task<ICollection<Permissao>> GetAllPermissaoByRoleIdAsync(Guid roleId)
    {
        return await _context.Permissoes
            .AsNoTracking()
            .Where(p => p.Roles.Any(r => r.Id == roleId)) 
            .OrderBy(p => p.Recurso)
            .Include(p => p.Roles)
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
