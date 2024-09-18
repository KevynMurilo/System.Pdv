using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Roles;

public interface ICreateRoleUseCase
{
    Task<OperationResult<Role>> ExecuteAsync(RoleDto roleDto);
}
