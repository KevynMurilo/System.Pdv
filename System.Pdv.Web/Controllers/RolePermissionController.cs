using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Authorization;
using System.Pdv.Application.Interfaces.Permissoes;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolePermissionController : ControllerBase
{
    private readonly IAssignPermissionToRoleUseCase _assignPermissionUseCase;
    private readonly ICreatePermissionUseCase _createPermissionUseCase;
    private readonly IRemovePermissionFromRoleUseCase _removePermissionUseCase;
    private readonly IGetAllPermissionUseCase _getAllPermissionUseCase;
    private readonly ILogger<RolePermissionController> _logger;

    public RolePermissionController(
        IAssignPermissionToRoleUseCase assignPermissionUseCase,
        IRemovePermissionFromRoleUseCase removePermissionUseCase,
        ICreatePermissionUseCase createPermissionUseCase,
        IGetAllPermissionUseCase getAllPermissionUseCase,
        ILogger<RolePermissionController> logger)
    {
        _assignPermissionUseCase = assignPermissionUseCase;
        _removePermissionUseCase = removePermissionUseCase;
        _getAllPermissionUseCase = getAllPermissionUseCase;
        _createPermissionUseCase = createPermissionUseCase;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPermissoes()
    {
        try
        {
            var result = await _getAllPermissionUseCase.ExecuteAsync();
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar roler");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpPost("permissao")]
    public async Task<IActionResult> CreatePermissao(CreatePermissionDto permissionDto)
    {
        try
        {
            var result = await _createPermissionUseCase.ExecuteAsync(permissionDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao criar permissao");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignPermissionToRole([FromBody] PermissionHasRoleDto permissaoDto)
    {
        var success = await _assignPermissionUseCase.ExecuteAsync(permissaoDto.RoleId, permissaoDto.PermissaoId);
        return success ? Ok("Permissão atribuída com sucesso.") : BadRequest("Erro ao atribuir permissão.");
    }

    [HttpPost("remove")]
    public async Task<IActionResult> RemovePermissionFromRole([FromBody] PermissionHasRoleDto permissaoDto)
    {
        var success = await _removePermissionUseCase.ExecuteAsync(permissaoDto.RoleId, permissaoDto.PermissaoId);
        return success ? Ok("Permissão removida com sucesso.") : BadRequest("Erro ao remover permissão.");
    }
}
