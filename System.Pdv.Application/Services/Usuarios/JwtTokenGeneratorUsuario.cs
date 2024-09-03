using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Pdv.Application.Interfaces.Usuarios;
using System.Pdv.Core.Entities;
using System.Security.Claims;
using System.Text;

namespace System.Pdv.Application.Services.Usuarios;

public class JwtTokenGeneratorUsuario : IJwtTokenGeneratorUsuario
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<JwtTokenGeneratorUsuario> _logger;

    public JwtTokenGeneratorUsuario(
        IConfiguration configuration,
        ILogger<JwtTokenGeneratorUsuario> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public string GenerateToken(Usuario usuario)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", usuario.Id.ToString()),
                    new Claim("nome", usuario.Nome),
                    new Claim("email", usuario.Email),
                    new Claim("role", usuario.Role.Nome), //Só é possivel pegar o Role.Nome por causa do include do GetByEmail de usuario
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao gerar o token para o usuário.");
            return string.Empty;
        }
    }
}
