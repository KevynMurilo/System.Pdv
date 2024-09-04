using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Auth;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutenticaçãoController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AutenticaçãoController> _logger;

    public AutenticaçãoController(
        IAuthService authService,
        ILogger<AutenticaçãoController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var token = await _authService.ExecuteAsync(loginDto);

            if (token == null) return Unauthorized();

            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao fazer login");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }
}
