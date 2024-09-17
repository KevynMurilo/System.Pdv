using System.Pdv.Application.Common;

namespace System.Pdv.Application.Interfaces.Permissoes;

public interface IRemovePermissionFromRoleUseCase
{
    Task<OperationResult<bool>> ExecuteAsync(Guid roleId, Guid permissaoId);
}
