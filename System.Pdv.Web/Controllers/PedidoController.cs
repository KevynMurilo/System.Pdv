using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Application.UseCase.Pedidos;
using System.Pdv.Core.Entities;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{
    private readonly IGetAllPedidosUseCase _getAllPedidosService;
    private readonly IPrintPedidoByIdsUseCase _printPedidoByIdsUseCase;
    private readonly IGetPedidosByMesaUseCase _getPedidosByMesaUseCase;
    private readonly IGetByIdPedidoUseCase _getByIdPedidoService;
    private readonly ICreatePedidoUseCase _createPedidoInternoService;
    private readonly IUpdatePedidoUseCase _updatePedidoInternoService;
    private readonly IDeletePedidoUseCase _deletePedidoService;
    private readonly ILogger<PedidoController> _logger;

    public PedidoController(
        IGetAllPedidosUseCase getAllPedidosService,
        IPrintPedidoByIdsUseCase printPedidoByIdsUseCase,
        IGetPedidosByMesaUseCase getPedidosByMesaUseCase,
        IGetByIdPedidoUseCase getByIdPedidoService,
        ICreatePedidoUseCase createPedidoInternoService,
        IUpdatePedidoUseCase updatePedidoInternoService,
        IDeletePedidoUseCase deletePedidoService,
        ILogger<PedidoController> logger)
    {
        _getAllPedidosService = getAllPedidosService;
        _printPedidoByIdsUseCase = printPedidoByIdsUseCase;
        _getPedidosByMesaUseCase = getPedidosByMesaUseCase;
        _getByIdPedidoService = getByIdPedidoService;
        _createPedidoInternoService = createPedidoInternoService;
        _updatePedidoInternoService = updatePedidoInternoService;
        _deletePedidoService = deletePedidoService;
        _logger = logger;
    }

    [Authorize(Roles = "ADMIN, GARCOM")]
    [HttpGet]
    public async Task<IActionResult> GetAllPedidos([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string tipoPedido = null, [FromQuery] string statusPedido = null)
    {
        try
        {
            var result = await _getAllPedidosService.ExecuteAsync(pageNumber, pageSize, tipoPedido, statusPedido);
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
    [HttpGet("{numeroMesa:int}")]
    public async Task<IActionResult> GetPedidoByMesa(int numeroMesa, [FromQuery] string statusPedido = null)
    {
        try
        {
            var result = await _getPedidosByMesaUseCase.ExecuteAsync(numeroMesa, statusPedido);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar pedidos de uma mesa especifica");
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
    [HttpPost("print")]
    public async Task<IActionResult> PrintPedidosByIds([FromBody] List<Guid> ids)
    {
        try
        {
            var result = await _printPedidoByIdsUseCase.ExecuteAsync(ids);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao imprimir pedidos pelos IDs fornecidos");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [Authorize(Roles = "ADMIN, GARCOM")]
    [HttpPost]
    public async Task<IActionResult> CreatePedido([FromBody] PedidoDto pedidoDto)
    {
        try
        {
            var result = await _createPedidoInternoService.ExecuteAsync(pedidoDto, User);
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
            var result = await _updatePedidoInternoService.ExecuteAsync(id, pedidoDto, User);
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
