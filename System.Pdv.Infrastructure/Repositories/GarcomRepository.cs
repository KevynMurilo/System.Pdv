using Microsoft.EntityFrameworkCore;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Data;

namespace System.Pdv.Infrastructure.Repositories;

public class GarcomRepository : IGarcomRepository
{
    private readonly AppDbContext _context;
    public GarcomRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Garcom>> GetAllAsync()
    {
        return await _context.Garcons
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Garcom?> GetByIdAsync(Guid id)
    {
        return await _context.Garcons.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task AddAsync(Garcom garcom)
    {
        _context.Garcons.Add(garcom);
        await _context.SaveChangesAsync();
    }
}
