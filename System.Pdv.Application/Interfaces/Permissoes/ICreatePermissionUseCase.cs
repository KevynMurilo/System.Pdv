using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Permissoes;

public interface ICreatePermissionUseCase
{
    Task<OperationResult<Permissao>> ExecuteAsync(CreatePermissionDto permissionDto);
}
