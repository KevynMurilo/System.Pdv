using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Permissoes;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissaoController : ControllerBase
{
    private readonly IGetAllPermissaoUseCase _getAllPermissionUseCase;
    private readonly ICreatePermissaoUseCase _createPermissionUseCase;
    private readonly ILogger<PermissaoController> _logger;

    public PermissaoController(
        IGetAllPermissaoUseCase getAllPermissionUseCase,
        ICreatePermissaoUseCase createPermissionUseCase,
        ILogger<PermissaoController> logger)
    {
        _getAllPermissionUseCase = getAllPermissionUseCase;
        _createPermissionUseCase = createPermissionUseCase;
        _logger = logger;
    }

    [HasPermission("RolePermission", "Get")]
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

    [HasPermission("RolePermission", "Create")]
    [HttpPost()]
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
}
