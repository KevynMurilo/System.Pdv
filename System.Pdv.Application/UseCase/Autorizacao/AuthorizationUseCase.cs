using System.Pdv.Application.Interfaces.Authorization;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Autorizacao;

public class AuthorizationUseCase : IAuthorizationUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public AuthorizationUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<bool> HasPermissionAsync(Guid userId, string recurso, string acao)
    {
        Console.WriteLine($"----------------------------------________________{userId}");
        var usuario = await _usuarioRepository.GetByIdAsync(userId);

        // Verifica se o usuário tem a permissão para o recurso e ação
        return usuario.Role.Permissoes.Any(p => p.Recurso == recurso && p.Acao == acao);
    }
}
