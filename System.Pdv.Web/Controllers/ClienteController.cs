using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Clientes;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ClienteController : ControllerBase
{
    private readonly ICreateClienteService _createClienteService;
    private readonly ILogger<ClienteController> _logger;
    public ClienteController(
        ICreateClienteService createClienteService,
        ILogger<ClienteController> logger)
    {
        _createClienteService = createClienteService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCliente(ClienteDto clienteDto)
    {
        try
        {
            var result = await _createClienteService.CreateCliente(clienteDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao cadastrar cliente");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }
}
