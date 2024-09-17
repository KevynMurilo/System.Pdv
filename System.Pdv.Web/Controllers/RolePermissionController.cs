using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Permissoes;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolePermissionController : ControllerBase
{
    private readonly IAssignPermissionToRoleUseCase _assignPermissionUseCase;
    private readonly IRemovePermissionFromRoleUseCase _removePermissionUseCase;
    private readonly ILogger<RolePermissionController> _logger;

    public RolePermissionController(
        IAssignPermissionToRoleUseCase assignPermissionUseCase,
        IRemovePermissionFromRoleUseCase removePermissionUseCase,
        ILogger<RolePermissionController> logger)
    {
        _assignPermissionUseCase = assignPermissionUseCase;
        _removePermissionUseCase = removePermissionUseCase;
        _logger = logger;
    }

    [HasPermission("RolePermission", "Create")]
    [HttpPost("atribuir")]
    public async Task<IActionResult> AssignPermissionToRole([FromBody] PermissionHasRoleDto permissaoDto)
    {
        try
        {
            var result = await _assignPermissionUseCase.ExecuteAsync(permissaoDto.RoleId, permissaoDto.PermissaoId);
            return result.StatusCode == 200
                ? Ok("Permissão atribuída com sucesso.")
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao vincular role e permissão");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HasPermission("RolePermission", "Delete")]
    [HttpDelete("remover")]
    public async Task<IActionResult> RemovePermissionFromRole([FromBody] PermissionHasRoleDto permissaoDto)
    {
        try
        {
            var result = await _removePermissionUseCase.ExecuteAsync(permissaoDto.RoleId, permissaoDto.PermissaoId);
            return result.StatusCode == 200
                ? Ok("Permissão removida com sucesso.")
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao remover vinculo de role e permissão");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }
}
