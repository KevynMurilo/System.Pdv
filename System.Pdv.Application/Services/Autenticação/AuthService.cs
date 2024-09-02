using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Auth;
using System.Pdv.Application.Interfaces.Authentication;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IJwtTokenGeneratorUsuario _tokenGeneratorGarcom;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IJwtTokenGeneratorUsuario tokenGeneratorGarcom,
        ILogger<AuthService> logger)
    {
        _usuarioRepository = usuarioRepository;
        _tokenGeneratorGarcom = tokenGeneratorGarcom;
        _logger = logger;
    }

    public async Task<OperationResult<string>> AuthenticateAsync(LoginDto loginDto)
    {
        try
        {
            var usuario = await _usuarioRepository.GetByEmail(loginDto.Email);
            if (usuario != null && BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.PasswordHash))
            {
                return new OperationResult<string> { Result = _tokenGeneratorGarcom.GenerateToken(usuario) };
            }

            return new OperationResult<string> { Message = "Credenciais inválidas", StatusCode = 401 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao logar usuário");
            return new OperationResult<string> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}