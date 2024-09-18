using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Usuarios;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
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

    [HttpGet]
    [HasPermission("Usuario", "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    [HttpGet("{id:guid}")]
    [HasPermission("Usuario", "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    [HttpPost]
    [HasPermission("Usuario", "Create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    [HttpPatch("{id:guid}")]
    [HasPermission("Usuario", "Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    [HttpDelete("{id:guid}")]
    [HasPermission("Usuario", "Delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
