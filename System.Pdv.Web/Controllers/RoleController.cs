using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Roles;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IGetAllRolesUseCase _getAllRoleServices;
    private readonly ICreateRoleUseCase _createRoleServices;
    private readonly IGetByIdRoleUseCase _getByIdRoleServices;
    private readonly IUpdateRoleUseCase _updateRoleServices;
    private readonly IDeleteRoleUseCase _deleteRoleServices;
    private readonly ILogger<RoleController> _logger;
    public RoleController(
        IGetAllRolesUseCase getAllRoleService,
        IGetByIdRoleUseCase getByIdRoleServices,
        ICreateRoleUseCase createRoleServices,
        IUpdateRoleUseCase updateRoleServices,
        IDeleteRoleUseCase deleteRoleServices,
        ILogger<RoleController> logger)
    {
        _getAllRoleServices = getAllRoleService;
        _getByIdRoleServices = getByIdRoleServices;
        _createRoleServices = createRoleServices;
        _updateRoleServices = updateRoleServices;   
        _deleteRoleServices = deleteRoleServices;   
        _logger = logger;
    }

    [HttpGet]
    [HasPermission("Role", "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRoles()
    {
        try
        {
            var result = await _getAllRoleServices.ExecuteAsync();
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

    [HttpGet("{id:guid}")]
    [HasPermission("Role", "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdRole(Guid id)
    {
        try
        {
            var result = await _getByIdRoleServices.ExecuteAsync(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar role por Id");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpPost]
    [HasPermission("Role", "Create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateRole(RoleDto roleDto)
    {
        try
        {
            var result = await _createRoleServices.ExecuteAsync(roleDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao cadastrar role");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpPatch("{id:guid}")]
    [HasPermission("Role", "Create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateRole(Guid id, RoleDto roleDto)
    {
        try
        {
            var result = await _updateRoleServices.ExecuteAsync(id, roleDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atualizar role");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpDelete("{id:guid}")]
    [HasPermission("Role", "Delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        try
        {
            var result = await _deleteRoleServices.ExecuteAsync(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao deletar role");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }
}
