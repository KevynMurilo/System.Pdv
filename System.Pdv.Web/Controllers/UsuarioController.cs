using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Usuarios;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IGetAllUsuarioUseCase _getAllUsuarioService;
    private readonly IGetByIdUsuarioUseCase _getByIdUsuarioService;
    private readonly ICreateUsuarioUseCase _createUsuarioService;
    private readonly IUpdateUsuarioUseCase _updateUsuarioService;
    private readonly IDeleteUsuarioUseCase _deleteGUsuarioService;
    private readonly ILogger<UsuarioController> _logger;
    public UsuarioController(
        IGetAllUsuarioUseCase getAllUsuarioService,
        IGetByIdUsuarioUseCase getByIdUsuarioService,
        ICreateUsuarioUseCase createUsuarioService,
        IUpdateUsuarioUseCase updateUsuarioService,
        IDeleteUsuarioUseCase deleteUsuarioService,
        ILogger<UsuarioController> logger)
    {
        _getAllUsuarioService = getAllUsuarioService;
        _getByIdUsuarioService = getByIdUsuarioService;
        _createUsuarioService = createUsuarioService;
        _updateUsuarioService = updateUsuarioService;
        _deleteGUsuarioService = deleteUsuarioService;
        _logger = logger;
    }

    [HasPermission("Usuario", "Get")]
    [HttpGet]
    public async Task<IActionResult> GetAllUsuario([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _getAllUsuarioService.ExecuteAsync(pageNumber, pageSize);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar usuários");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HasPermission("Usuario", "Get")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var result = await _getByIdUsuarioService.ExecuteAsync(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao procurar usuário por id");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HasPermission("Usuario", "Create")]
    [HttpPost]
    public async Task<IActionResult> AddUsuario(UsuarioDto usuarioDto)
    {
        try
        {
            var result = await _createUsuarioService.ExecuteAsync(usuarioDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao registrar usuário");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HasPermission("Usuario", "Update")]
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateUsuario(Guid id, UsuarioDto usuarioDto)
    {
        try
        {
            var result = await _updateUsuarioService.ExecuteAsync(id, usuarioDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atualizar usuário");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HasPermission("Usuario", "Delete")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUsuario(Guid id)
    {
        try
        {
            var result = await _deleteGUsuarioService.ExecuteAsync(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao deletar usuário");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }
}
