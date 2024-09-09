using Microsoft.EntityFrameworkCore;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Data;

namespace System.Pdv.Infrastructure.Repositories;

public class StatusPedidoRepository : IStatusPedidoRepository
{
    private readonly AppDbContext _context;
    
    public StatusPedidoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(StatusPedido statusPedido)
    {
        _context.StatusPedidos.Add(statusPedido);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<StatusPedido>> GetAllAsync()
    {
        return await _context.StatusPedidos
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<StatusPedido?> GetByIdAsync(Guid id)
    {
        return await _context.StatusPedidos
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<StatusPedido?> GetByNameAsync(string status)
    {
        return await _context.StatusPedidos
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Status == status.ToUpper());
    }

    public async Task DeleteAsync(StatusPedido statusPedido)
    {
        _context.StatusPedidos.Remove(statusPedido);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(StatusPedido statusPedido)
    {
        _context.StatusPedidos.Update(statusPedido);
        await _context.SaveChangesAsync();
    }
}
