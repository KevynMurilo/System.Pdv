using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Auth;
using System.Pdv.Application.Interfaces.Usuarios;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Auth;
public class AuthUseCase : IAuthUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IJwtTokenGeneratorUsuario _tokenGeneratorGarcom;
    private readonly ILogger<AuthUseCase> _logger;

    public AuthUseCase(
        IUsuarioRepository usuarioRepository,
        IJwtTokenGeneratorUsuario tokenGeneratorGarcom,
        ILogger<AuthUseCase> logger)
    {
        _usuarioRepository = usuarioRepository;
        _tokenGeneratorGarcom = tokenGeneratorGarcom;
        _logger = logger;
    }

    public async Task<OperationResult<dynamic>> ExecuteAsync(LoginDto loginDto)
    {
        try
        {
            var usuario = await _usuarioRepository.GetByEmail(loginDto.Email);
            if (usuario != null && BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.PasswordHash))
            {
                string token = _tokenGeneratorGarcom.GenerateToken(usuario);

                var useWithToken = new { usuario, token };

                return new OperationResult<dynamic> { Result = useWithToken };
            }

            return new OperationResult<dynamic> { Message = "Credenciais inválidas", StatusCode = 401 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao logar usuário");
            return new OperationResult<dynamic> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}