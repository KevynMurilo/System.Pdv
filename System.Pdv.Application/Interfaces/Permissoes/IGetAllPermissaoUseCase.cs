using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Permissoes;

public interface IGetAllPermissaoUseCase
{
    Task<OperationResult<IEnumerable<Permissao>>> ExecuteAsync();
}
