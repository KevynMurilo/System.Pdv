using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;

namespace System.Pdv.Application.Interfaces.Permissoes;

public interface IRemovePermissionFromRoleUseCase
{
    Task<OperationResult<bool>> ExecuteAsync(PermissionHasRoleDto permissionHasRoleDto);
}
