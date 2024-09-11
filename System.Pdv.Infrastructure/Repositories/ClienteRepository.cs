using Microsoft.EntityFrameworkCore;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Data;

namespace System.Pdv.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _context.Clientes
                .OrderBy(c => c.Nome)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Cliente?> GetByIdAsync(Guid id)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Cliente>> GetByNameAsync(string nome)
        {
            return await _context.Clientes
                .Where(c => c.Nome.ToLower().Contains(nome.ToLower()))
                .ToListAsync();
        }

        public async Task DeleteAsync(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }
    }
}
