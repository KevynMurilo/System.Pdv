using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Authentication;

public interface IJwtTokenGeneratorUsuario
{
    string GenerateToken(Usuario usuario);
}
