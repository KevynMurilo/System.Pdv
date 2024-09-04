using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Usuarios;

public interface IGetByIdUsuarioService
{
    Task<OperationResult<Usuario>> ExecuteAsync(Guid id);
}
