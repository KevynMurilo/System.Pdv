using System.Pdv.Core.Entities;

namespace System.Pdv.Core.Interfaces;

public interface IClienteRepository
{
    Task<Cliente> AddAsync(Cliente cliente);
}
