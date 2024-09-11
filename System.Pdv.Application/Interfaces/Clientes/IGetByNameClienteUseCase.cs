using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Clientes;

public interface IGetByNameClienteUseCase
{
    Task<OperationResult<IEnumerable<Cliente>>> ExecuteAsync(string nome);
}
