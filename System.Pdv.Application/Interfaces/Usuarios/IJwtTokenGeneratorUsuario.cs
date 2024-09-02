using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Usuarios;

public interface IJwtTokenGeneratorUsuario
{
    string GenerateToken(Usuario usuario);
}
