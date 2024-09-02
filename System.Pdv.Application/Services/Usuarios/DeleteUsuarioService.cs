using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Usuarios;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Usuarios;

public class DeleteUsuarioService : IDeleteUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ILogger<DeleteUsuarioService> _logger;

    public DeleteUsuarioService(
        IUsuarioRepository usuarioRepository,
        ILogger<DeleteUsuarioService> logger)
    {
        _usuarioRepository = usuarioRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Usuario>> DeleteUsuario(Guid id)
    {
        try
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                return new OperationResult<Usuario> { Message = "Usuário não encontrado", StatusCode = 404 };

            await _usuarioRepository.DeleteAsync(usuario);
            return new OperationResult<Usuario> { Result = usuario, Message = "Usuário deletado com sucesso" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao deletar usuário");
            return new OperationResult<Usuario> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
