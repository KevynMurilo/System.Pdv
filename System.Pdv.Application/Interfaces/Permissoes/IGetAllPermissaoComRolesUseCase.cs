using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Permissoes;

public interface IGetAllPermissaoComRolesUseCase
{
    Task<OperationResult<IEnumerable<Permissao>>> ExecuteAsync(int pageNumber, int pageSize, string recurso, string acao);
}
