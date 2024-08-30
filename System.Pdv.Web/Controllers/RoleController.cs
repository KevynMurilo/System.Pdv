using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Roles;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class RoleController : ControllerBase
{
    private readonly IGetAllRolesService _getAllRoleServices;
    private readonly ICreateRoleService _createRoleService;
    private readonly ILogger<RoleController> _logger;
    public RoleController(
        ICreateRoleService createRoleService,
        IGetAllRolesService getAllRoleService,
        ILogger<RoleController> logger)
    {
        _getAllRoleServices = getAllRoleService;
        _createRoleService = createRoleService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        try
        {
            var result = await _getAllRoleServices.GetAllRoles();
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

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateRole(RoleDto roleDto)
    {
        try
        {
            var result = await _createRoleService.CreateRole(roleDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao criar role");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }
}
