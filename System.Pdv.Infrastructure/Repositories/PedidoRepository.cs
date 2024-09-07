using Microsoft.EntityFrameworkCore;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Data;

namespace System.Pdv.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly AppDbContext _context;
    
    public PedidoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pedido>> GetPedidosAsync(int pageNumber, int pageSize, string type)
    {
        IQueryable<Pedido> query = _context.Pedidos
            .AsNoTracking()
            .OrderByDescending(p => p.DataHora)
            .Include(i => i.Items)
                .ThenInclude(i => i.Adicionais);

        switch (type.ToLower())
        {
            case "interno":
                query = query.Where(c => c.ClienteId == null);
                break;
            case "externo":
                query = query.Where(m => m.MesaId == null);
                break;
            default:
                break;
        }

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    // Removido o AsNoTracking para que o Entity Framework rastreie as entidades,
    // permitindo que as entidades associadas (como Itens e Adicionais) possam ser atualizadas
    // sem gerar erros de duplicidade ou conflitos no rastreamento de entidades.
    public async Task<Pedido?> GetByIdAsync(Guid id)
    {
        return await _context.Pedidos
            .Include(i => i.Items)
                .ThenInclude(i => i.Adicionais)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Pedido> AddAsync(Pedido pedido)
    {
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
        return pedido;
    }

    public async Task RemoveItem(ItemPedido item)
    {
        _context.ItensPedidos.Remove(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Pedido pedido)
    {
        _context.Pedidos.Update(pedido);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Pedido pedido)
    {
        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();
    }
}
