using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Roles;

public interface IGetAllRolesUseCase
{
    Task<OperationResult<IEnumerable<Role>>> ExecuteAsync();
}
