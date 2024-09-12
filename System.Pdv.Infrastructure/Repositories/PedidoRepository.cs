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

    public async Task<IEnumerable<Pedido>> GetPedidosAsync(int pageNumber, int pageSize, string tipoPedido, string statusPedido)
    {
        IQueryable<Pedido> query = _context.Pedidos
            .AsNoTracking()
            .OrderByDescending(p => p.DataHora)
            .Include(m => m.Mesa)
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
        if (!string.IsNullOrEmpty(tipoPedido))
        {
            switch (tipoPedido.ToLower())
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
        if (!string.IsNullOrEmpty(statusPedido))
        {
            query = query.Where(s => s.StatusPedido.Status == statusPedido.ToUpper());
        }

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Pedido>> GetPedidosByMesaAsync(int numeroMesa, string statusPedido)
    {
        IQueryable<Pedido> query = _context.Pedidos
            .Where(p => p.Mesa.Numero == numeroMesa)
            .Include(m => m.Mesa)
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

        // Filtra por status de pedido
        if (!string.IsNullOrEmpty(statusPedido))
        {
            query = query.Where(p => p.StatusPedido.Status == statusPedido.ToUpper());
        }

        return await query.ToListAsync();
    }

    // Removido o AsNoTracking para que o Entity Framework rastreie as entidades,
    // permitindo que as entidades associadas (como Itens e Adicionais) possam ser atualizadas
    // sem gerar erros de duplicidade ou conflitos no rastreamento de entidades.
    public async Task<Pedido?> GetByIdAsync(Guid id)
    {
        return await _context.Pedidos
            .Include(c => c.Cliente)
            .Include(m => m.Mesa)
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

    public async Task<List<Pedido>> GetPedidosByIdsAsync(IEnumerable<Guid> ids)
    {
        return await _context.Pedidos
            .Where(p => ids.Contains(p.Id))
            .Include(c => c.Cliente)
            .Include(m => m.Mesa)
            .Include(g => g.Garcom)
                .ThenInclude(r => r.Role)
            .Include(m => m.MetodoPagamento)
            .Include(s => s.StatusPedido)
            .Include(i => i.Items)
                .ThenInclude(p => p.Produto)
                    .ThenInclude(c => c.Categoria)
            .Include(i => i.Items)
                .ThenInclude(i => i.Adicionais)
            .ToListAsync();
    }

    //Criei esse método de cliente aqui por que vou usar ele só em pedidos!
    public async Task<Cliente?> GetClienteByNomeTelefoneAsync(string nome, string telefone)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Nome == nome && c.Telefone == telefone);
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
