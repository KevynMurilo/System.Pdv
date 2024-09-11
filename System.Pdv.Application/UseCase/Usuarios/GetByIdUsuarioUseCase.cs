using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Usuarios;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Usuarios;

public class GetByIdUsuarioUseCase : IGetByIdUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ILogger<GetByIdUsuarioUseCase> _logger;

    public GetByIdUsuarioUseCase(
        IUsuarioRepository usuarioRepository,
        ILogger<GetByIdUsuarioUseCase> logger)
    {
        _usuarioRepository = usuarioRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Usuario>> ExecuteAsync(Guid id)
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
