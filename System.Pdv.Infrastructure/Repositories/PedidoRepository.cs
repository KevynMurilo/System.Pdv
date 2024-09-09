using Microsoft.EntityFrameworkCore;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
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

    public async Task<IEnumerable<Pedido>> GetPedidosAsync(int pageNumber, int pageSize, string type, string status)
    {
        IQueryable<Pedido> query = _context.Pedidos
            .AsNoTracking()
            .OrderByDescending(p => p.DataHora)
            .Include(c => c.Cliente)
            .Include(g => g.Garcom)
                .ThenInclude(r => r.Role)
            .Include(m => m.MetodoPagamento)
            .Include(s => s.StatusPedido)
            .Include(i => i.Items)
                .ThenInclude(p => p.Produto)
                    .ThenInclude(c => c.Categoria)
            .Include(i => i.Items)
                .ThenInclude(i => i.Adicionais);

        // Filtra por tipo de pedido
        if (!string.IsNullOrEmpty(type))
        {
            switch (type.ToLower())
            {
                case "interno":
                    query = query.Where(t => t.TipoPedido == TipoPedido.Interno);
                    break;
                case "externo":
                    query = query.Where(t => t.TipoPedido == TipoPedido.Externo);
                    break;
                default:
                    break;
            }
        }
        // Filtra por status de pedido
        if (!string.IsNullOrEmpty(status) && !status.Equals("todos", StringComparison.OrdinalIgnoreCase))
        {
            query = query.Where(s => s.StatusPedido.Status == status.ToUpper());
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
            .Include(c => c.Cliente)
            .Include(g => g.Garcom)
                .ThenInclude(r => r.Role)
            .Include(m => m.MetodoPagamento)
            .Include(s => s.StatusPedido)
            .Include(i => i.Items)
                .ThenInclude(p => p.Produto)
                    .ThenInclude(c => c.Categoria)
            .Include(i => i.Items)
                .ThenInclude(i => i.Adicionais)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Pedido pedido)
    {
        _context.Pedidos.Add(pedido);
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
    public async Task RemoveItem(ItemPedido item)
    {
        _context.ItensPedidos.Remove(item);
        await _context.SaveChangesAsync();
    }

}
