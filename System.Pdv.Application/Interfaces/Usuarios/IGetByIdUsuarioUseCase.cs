using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Usuarios;

public interface IGetByIdUsuarioUseCase
{
    Task<OperationResult<Usuario>> ExecuteAsync(Guid id);
}
