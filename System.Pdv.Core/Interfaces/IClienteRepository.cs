using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IClienteRepository
{
    Task<IEnumerable<Cliente>> GetAllAsync(int pageNumber, int pageSize);
    Task<Cliente> GetByIdAsync(Guid id);
    Task<IEnumerable<Cliente>> GetByNameAsync(string nome);
    Task DeleteAsync(Cliente itemAdicional);
}
