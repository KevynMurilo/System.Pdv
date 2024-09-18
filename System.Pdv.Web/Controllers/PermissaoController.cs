using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.Interfaces.Permissoes;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissaoController : ControllerBase
{
    private readonly IGetAllPermissaoUseCase _getAllPermissaoUseCase;
    private readonly IGetAllPermissaoComRolesUseCase _getAllPermissaoComRoleUseCase;
    private readonly IGetAllPermissaoByRoleIdUseCase _getAllPermissaoByRoleIdUseCase;
    private readonly ILogger<PermissaoController> _logger;

    public PermissaoController(
        IGetAllPermissaoUseCase getAllPermissaoUseCase,
        IGetAllPermissaoComRolesUseCase getAllPermissaoComRolesUseCase,
        IGetAllPermissaoByRoleIdUseCase getAllPermissaoByRoleIdUseCase,
        ILogger<PermissaoController> logger)
    {
        _getAllPermissaoUseCase = getAllPermissaoUseCase;
        _getAllPermissaoComRoleUseCase = getAllPermissaoComRolesUseCase;
        _getAllPermissaoByRoleIdUseCase = getAllPermissaoByRoleIdUseCase;
        _logger = logger;
    }

    [HttpGet()]
    [HasPermission("RolePermission", "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPermissoes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? recurso = null, [FromQuery] string? acao = null)
    {
        try
        {
            var result = await _getAllPermissaoUseCase.ExecuteAsync(pageNumber, pageSize, recurso, acao);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar roles");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpGet("com/roles")]
    [HasPermission("RolePermission", "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPermissoesComRoles([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? recurso = null, [FromQuery] string? acao = null)
    {
        try
        {
            var result = await _getAllPermissaoComRoleUseCase.ExecuteAsync(pageNumber, pageSize, recurso, acao);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar roles");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpGet("role/{roleId:guid}")]
    [HasPermission("RolePermission", "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPermissaoByRoleId(Guid roleId)
    {
        try
        {
            var result = await _getAllPermissaoByRoleIdUseCase.ExecuteAsync(roleId);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar roles por id");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }
}
