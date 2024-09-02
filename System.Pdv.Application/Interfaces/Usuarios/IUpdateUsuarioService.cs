using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Usuarios;

public interface IUpdateUsuarioService
{
    Task<OperationResult<Usuario>> UpdateUsuario(Guid id, UsuarioDto usuario);
}
