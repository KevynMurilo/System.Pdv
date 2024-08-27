using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Clientes;

public interface ICreateClienteService
{
    Task<OperationResult<Cliente>> CreateCliente(ClienteDto cliente);
}
