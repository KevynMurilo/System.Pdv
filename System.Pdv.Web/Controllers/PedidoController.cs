using Microsoft.AspNetCore.Authorization;
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
    private readonly ICreatePedidoService _createPedidoInternoService;
    private readonly IUpdatePedidoService _updatePedidoInternoService;
    private readonly IDeletePedidoService _deletePedidoService;
    private readonly ILogger<PedidoController> _logger;

    public PedidoController(
        IGetAllPedidosServices getAllPedidosService,
        IGetByIdPedidoService getByIdPedidoService,
        ICreatePedidoService createPedidoInternoService,
        IUpdatePedidoService updatePedidoInternoService,
        IDeletePedidoService deletePedidoService,
        ILogger<PedidoController> logger)
    {
        _getAllPedidosService = getAllPedidosService;
        _getByIdPedidoService = getByIdPedidoService;
        _createPedidoInternoService = createPedidoInternoService;
        _updatePedidoInternoService = updatePedidoInternoService;
        _deletePedidoService = deletePedidoService;
        _logger = logger;
    }

    [Authorize(Roles = "ADMIN, GARCOM")]
    [HttpGet]
    public async Task<IActionResult> GetAllPedidos([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string type = "todos", [FromQuery] string status = "todos")
    {
        try
        {
            var result = await _getAllPedidosService.ExecuteAsync(pageNumber, pageSize, type, status);
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

    [Authorize(Roles = "ADMIN, GARCOM")]
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

    [Authorize(Roles = "ADMIN, GARCOM")]
    [HttpPost]
    public async Task<IActionResult> CreatePedido([FromBody] PedidoDto pedidoDto)
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
            _logger.LogError(ex, "Ocorreu um erro ao registrar pedido");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [Authorize(Roles = "ADMIN, GARCOM")]
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdatePedido(Guid id, PedidoDto pedidoDto)
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
            _logger.LogError(ex, "Ocorreu um erro ao atualizar pedido");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [Authorize(Roles = "ADMIN, GARCOM")]
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
