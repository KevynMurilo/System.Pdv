using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Pedidos.Externos;
using System.Pdv.Application.Interfaces.Pedidos.Internos;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{
    private readonly ICreatePedidoInternoService _createPedidoInternoService;
    private readonly ICreatePedidoExternoService _createPedidoExternoService;
    private readonly ILogger<PedidoController> _logger;

    public PedidoController(
        ICreatePedidoInternoService createPedidoInternoService,
        ICreatePedidoExternoService createPedidoExternoService,
        ILogger<PedidoController> logger)
    {
        _createPedidoInternoService = createPedidoInternoService;
        _createPedidoExternoService = createPedidoExternoService;
        _logger = logger;
    }

    [HttpPost("interno")]
    public async Task<IActionResult> CriarPedidoInterno([FromBody] PedidoInternoDto pedidoDto)
    {
        try
        {
            var result = await _createPedidoInternoService.ExecuteAsync(pedidoDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao registrar pedido interno");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpPost("externo")]
    public async Task<IActionResult> CriarPedidoExterno([FromBody] PedidoExternoDto pedidoDto)
    {
        try
        {
            var result = await _createPedidoExternoService.ExecuteAsync(pedidoDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao registrar pedido externo");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }


}
