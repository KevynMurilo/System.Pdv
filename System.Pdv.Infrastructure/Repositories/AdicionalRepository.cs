using Microsoft.EntityFrameworkCore;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Data;

namespace System.Pdv.Infrastructure.Repositories;

public class AdicionalRepository : IAdicionalRepository
{
    private readonly AppDbContext _context;
    public AdicionalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ItemAdicional itemAdicional)
    {
        _context.Adicionais.Add(itemAdicional);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ItemAdicional>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await _context.Adicionais
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<ItemAdicional?> GetByIdAsync(Guid id)
    {
        return await _context.Adicionais
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<ItemAdicional?> GetByNameAsync(string nome)
    {
        return await _context.Adicionais
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Nome == nome.ToUpper());
    }

    public async Task<List<ItemAdicional>> GetAdicionaisByIdsAsync(List<Guid> ids)
    {
        return await _context.Adicionais
            .AsNoTracking()
            .Where(adicional => ids.Contains(adicional.Id))
            .ToListAsync();
    }


    //É utilizado para informar ao contexto que uma entidade já existe no banco de dados e que não deve ser inserida novamente.
    public void AttachAdicionais(IEnumerable<ItemAdicional> adicionais)
    {
        foreach (var adicional in adicionais)
        {
            _context.Attach(adicional);
        }
    }

    public async Task DeleteAsync(ItemAdicional itemAdicional)
    {
        _context.Adicionais.Remove(itemAdicional);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ItemAdicional itemAdicional)
    {
        _context.Adicionais.Update(itemAdicional);
        await _context.SaveChangesAsync();
    }
}
