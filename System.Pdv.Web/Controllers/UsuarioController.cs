using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Usuarios;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IGetAllUsuarioService _getAllUsuarioService;
    private readonly IGetByIdUsuarioService _getByIdUsuarioService;
    private readonly ICreateUsuarioService _createUsuarioService;
    private readonly IUpdateUsuarioService _updateUsuarioService;
    private readonly IDeleteUsuarioService _deleteGUsuarioService;
    private readonly ILogger<ClienteController> _logger;
    public UsuarioController(
        IGetAllUsuarioService getAllUsuarioService,
        IGetByIdUsuarioService getByIdUsuarioService,
        ICreateUsuarioService createUsuarioService,
        IUpdateUsuarioService updateUsuarioService,
        IDeleteUsuarioService deleteUsuarioService,
        ILogger<ClienteController> logger)
    {
        _getAllUsuarioService = getAllUsuarioService;
        _getByIdUsuarioService = getByIdUsuarioService;
        _createUsuarioService = createUsuarioService;
        _updateUsuarioService = updateUsuarioService;
        _deleteGUsuarioService = deleteUsuarioService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsuario()
    {
        try
        {
            var result = await _getAllUsuarioService.GetAllUsuario();
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
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var result = await _getByIdUsuarioService.GetById(id);
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
    public async Task<IActionResult> AddUsuario(UsuarioDto usuarioDto)
    {
        try
        {
            var result = await _createUsuarioService.CreateUsuario(usuarioDto);
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
    public async Task<IActionResult> UpdateUsuario(Guid id, UsuarioDto usuarioDto)
    {
        try
        {
            var result = await _updateUsuarioService.UpdateUsuario(id, usuarioDto);
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
    public async Task<IActionResult> DeleteUsuario(Guid id)
    {
        try
        {
            var result = await _deleteGUsuarioService.DeleteUsuario(id);
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
