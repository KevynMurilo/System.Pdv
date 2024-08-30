using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Usuarios;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Usuarios;

public class GetByIdUsuarioService : IGetByIdUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ILogger<GetByIdUsuarioService> _logger;

    public GetByIdUsuarioService(
        IUsuarioRepository usuarioRepository,
        ILogger<GetByIdUsuarioService> logger)
    {
        _usuarioRepository = usuarioRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Usuario>> GetById(Guid id)
    {
        try
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                return new OperationResult<Usuario> { Message = "Usuário não encontrado", StatusCode = 404 };

            return new OperationResult<Usuario> { Result = usuario };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao procurar usuário por id");
            return new OperationResult<Usuario> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
