using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Pedidos;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{
    private readonly IGetAllPedidosServices _getAllPedidosService;
    private readonly IGetByIdPedidoService _getByIdPedidoService;
    private readonly ICreatePedidoInternoService _createPedidoInternoService;
    private readonly ICreatePedidoExternoService _createPedidoExternoService;
    private readonly IUpdatePedidoInternoService _updatePedidoInternoService;
    private readonly IDeletePedidoService _deletePedidoService;
    private readonly ILogger<PedidoController> _logger;

    public PedidoController(
        IGetAllPedidosServices getAllPedidosService,
        IGetByIdPedidoService getByIdPedidoService,
        ICreatePedidoInternoService createPedidoInternoService,
        ICreatePedidoExternoService createPedidoExternoService,
        IUpdatePedidoInternoService updatePedidoInternoService,
        IDeletePedidoService deletePedidoService,
        ILogger<PedidoController> logger)
    {
        _getAllPedidosService = getAllPedidosService;
        _getByIdPedidoService = getByIdPedidoService;
        _createPedidoInternoService = createPedidoInternoService;
        _createPedidoExternoService = createPedidoExternoService;
        _updatePedidoInternoService = updatePedidoInternoService;
        _deletePedidoService = deletePedidoService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPedidos([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string type = "todos")
    {
        try
        {
            var result = await _getAllPedidosService.ExecuteAsync(pageNumber, pageSize, type);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar pedidos");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdPedido(Guid id)
    {
        try
        {
            var result = await _getByIdPedidoService.ExecuteAsync(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao procurar pedido por id");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
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

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdatePedidoInterno(Guid id, PedidoInternoDto pedidoDto)
    {
        try
        {
            var result = await _updatePedidoInternoService.ExecuteAsync(id, pedidoDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atualizar pedido interno");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePedido(Guid id)
    {
        try
        {
            var result = await _deletePedidoService.ExecuteAsync(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao deletar pedido");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }
}
