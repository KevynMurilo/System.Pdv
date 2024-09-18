using System.ComponentModel.DataAnnotations;

namespace System.Pdv.Application.DTOs;

public class PermissionHasRoleDto
{
    public List<Guid> RoleIds { get; set; }
    public List<Guid> PermissaoIds { get; set; }
}
