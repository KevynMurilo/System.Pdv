using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Roles;

public interface ICreateRoleService
{
    Task<OperationResult<Role>> CreateRole(RoleDto roleDto);
}
