using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.StatusPedidos;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusPedidoController : ControllerBase
{
    private readonly IGetAllStatusPedidoUseCase _getAllStatusPedidoService;
    private readonly ICreateStatusPedidoUseCase _createStatusPedidoService;
    private readonly IGetByIdStatusPedidoUseCase _getByIdStatusPedidoService;
    private readonly IUpdateStatusPedidoUseCase _updateStatusPedidoService;
    private readonly IDeleteStatusPedidoUseCase _deleteStatusPedidoService;
    private readonly ILogger<StatusPedidoController> _logger;

    public StatusPedidoController(
        IGetAllStatusPedidoUseCase getAllStatusPedidoService,
        IGetByIdStatusPedidoUseCase getByIdStatusPedidoService,
        ICreateStatusPedidoUseCase statusPedidoService,
        IUpdateStatusPedidoUseCase updateStatusPedidoService,
        IDeleteStatusPedidoUseCase deleteStatusPedidoService,
        ILogger<StatusPedidoController> logger)
    {
        _getAllStatusPedidoService = getAllStatusPedidoService;
        _getByIdStatusPedidoService = getByIdStatusPedidoService;
        _createStatusPedidoService = statusPedidoService;
        _updateStatusPedidoService = updateStatusPedidoService;
        _deleteStatusPedidoService = deleteStatusPedidoService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStatusPedido()
    {
        try
        {
            var result = await _getAllStatusPedidoService.ExecuteAsync();
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar status de pedido");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdStatusPedido(Guid id)
    {
        try
        {
            var result = await _getByIdStatusPedidoService.ExecuteAsync(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao procurar status de pedido por id");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    public async Task<IActionResult> CreateStatusPedido(StatusPedidoDto statusPedidoDto)
    {
        try
        {
            var result = await _createStatusPedidoService.ExecuteAsync(statusPedidoDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao registrar status de pedido");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateStatusPedido(Guid id, StatusPedidoDto statusPedidoDto)
    {
        try
        {
            var result = await _updateStatusPedidoService.ExecuteAsync(id, statusPedidoDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atualizar status de pedido");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [Authorize(Roles = "ADMIN")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteStatusPedido(Guid id)
    {
        try
        {
            var result = await _deleteStatusPedidoService.ExecuteAsync(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao deletar status de pedido");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }
}
