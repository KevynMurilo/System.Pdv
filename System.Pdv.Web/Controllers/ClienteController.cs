using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Clientes;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ClienteController : ControllerBase
{
    private readonly ICreateClienteService _createClienteService;
    public ClienteController(ICreateClienteService createClienteService)
    {
        _createClienteService = createClienteService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCliente(ClienteDto clienteDto)
    {
        var cliente = await _createClienteService.CreateCliente(clienteDto);
        return Ok(cliente);
    }
}
