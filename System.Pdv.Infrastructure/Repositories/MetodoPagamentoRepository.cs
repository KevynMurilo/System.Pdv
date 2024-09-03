using Microsoft.EntityFrameworkCore;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Data;

namespace System.Pdv.Infrastructure.Repositories;

public class MetodoPagamentoRepository : IMetodoPagamentoRepository
{
    private readonly AppDbContext _context;
    public MetodoPagamentoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(MetodoPagamento metodoPagamento)
    {
        _context.MetodoPagamento.Add(metodoPagamento);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<MetodoPagamento>> GetAllAsync()
    {
        return await _context.MetodoPagamento
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<MetodoPagamento?> GetByIdAsync(Guid id)
    {
        return await _context.MetodoPagamento
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<MetodoPagamento?> GetByNameAsync(string nome)
    {
        return await _context.MetodoPagamento
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Nome == nome.ToUpper());
    }

    public async Task DeleteAsync(MetodoPagamento metodoPagamento)
    {
        _context.MetodoPagamento.Remove(metodoPagamento);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MetodoPagamento metodoPagamento)
    {
        _context.MetodoPagamento.Update(metodoPagamento);
        await _context.SaveChangesAsync();
    }
}
