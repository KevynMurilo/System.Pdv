using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Clientes;

public interface IGetByIdClienteUseCase
{
    Task<OperationResult<Cliente>> ExecuteAsync(Guid id);
}
