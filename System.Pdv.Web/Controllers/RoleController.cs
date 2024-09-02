﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Roles;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class RoleController : ControllerBase
{
    private readonly IGetAllRolesService _getAllRoleServices;
    private readonly ILogger<RoleController> _logger;
    public RoleController(
        IGetAllRolesService getAllRoleService,
        ILogger<RoleController> logger)
    {
        _getAllRoleServices = getAllRoleService;
        _logger = logger;
    }

    [Authorize(Roles = "ADMIN")]
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
}
