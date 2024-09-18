using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Roles;

public interface IUpdateRoleUseCase
{
    Task<OperationResult<Role>> ExecuteAsync(Guid id, RoleDto roledDto);
}
