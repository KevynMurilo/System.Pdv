using Microsoft.EntityFrameworkCore;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Data;

namespace System.Pdv.Infrastructure.Repositories;

public class MesaRepository : IMesaRepository
{
    private readonly AppDbContext _context;
    public MesaRepository(AppDbContext context)
    { 
        _context = context;
    }

    public async Task<Mesa> AddAsync(Mesa mesa)
    {
        _context.Mesas.Add(mesa);
        await _context.SaveChangesAsync();
        return mesa;
    }

    public async Task<IEnumerable<Mesa>> GetAllAsync()
    {
        return await _context.Mesas
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Mesa> GetByIdAsync(Guid id)
    {
        return await _context.Mesas
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Mesa> GetByNumberAsync(int numero)
    {
        return await _context.Mesas.AsNoTracking().FirstOrDefaultAsync(m => m.Numero == numero);
    }
}
