using Microsoft.Extensions.Logging;
using System.Pdv.Application.Interfaces.Authorization;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Autorizacao;

public class AuthorizationUseCase : IAuthorizationUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ILogger<AuthorizationUseCase> _logger;

    public AuthorizationUseCase(IUsuarioRepository usuarioRepository, ILogger<AuthorizationUseCase> logger)
    {
        _usuarioRepository = usuarioRepository;
        _logger = logger;
    }

    public async Task<bool> HasPermissionAsync(Guid userId, string recurso, string acao)
    {
        try
        {
            var usuario = await _usuarioRepository.GetByIdAsync(userId);

            if (usuario == null)
            {
                return false;
            }

            return usuario.Role.Permissoes.Any(p => p.Recurso == recurso && p.Acao == acao);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ocorreu um erro ao verificar as permissões do usuário com a ID {userId}.");
            return false;
        }
    }
}
