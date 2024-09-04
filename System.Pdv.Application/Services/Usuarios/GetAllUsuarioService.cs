using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Usuarios;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Usuarios;

public class GetAllUsuarioService : IGetAllUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ILogger<GetAllUsuarioService> _logger;
    public GetAllUsuarioService(
        IUsuarioRepository usuarioRepository,
        ILogger<GetAllUsuarioService> logger)
    {
        _usuarioRepository = usuarioRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<Usuario>>> ExecuteAsync(int pageNumber, int pageSize)
    {
        try
        {
            var usuarios = await _usuarioRepository.GetAllAsync(pageNumber, pageSize);
            if (!usuarios.Any())
                return new OperationResult<IEnumerable<Usuario>> { Message = "Nenhum usuário encontrado", StatusCode = 404 };

            return new OperationResult<IEnumerable<Usuario>> { Result = usuarios };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao buscar usuário");
            return new OperationResult<IEnumerable<Usuario>> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
