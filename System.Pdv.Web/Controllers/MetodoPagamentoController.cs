using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.MetodosPagamento;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetodoPagamentoController : ControllerBase
{
    private readonly IGetAllMetodoPagamentoServices _getAllMetodoPagamentoServices;
    private readonly IGetByIdMetodoPagamentoService _getByIdMetodoPagamentoService;
    private readonly ICreateMetodoPagamentoService _createMetodoPagamentoService;
    private readonly IUpdateMetodoPagamentoService _updateMetodoPagamentoService;
    private readonly IDeleteMetodoPagamentoService _deleteMetodoPagamentoService;
    private readonly ILogger<MetodoPagamentoController> _logger;

    public MetodoPagamentoController(
        IGetAllMetodoPagamentoServices getAllMetodoPagamentoServices,
        IGetByIdMetodoPagamentoService getByIdMetodoPagamentoService,
        ICreateMetodoPagamentoService createMetodoPagamentoService,
        IUpdateMetodoPagamentoService updateMetodoPagamentoService,
        IDeleteMetodoPagamentoService deleteMetodoPagamentoService,
        ILogger<MetodoPagamentoController> logger)
    {
        _getAllMetodoPagamentoServices = getAllMetodoPagamentoServices;
        _getByIdMetodoPagamentoService = getByIdMetodoPagamentoService;
        _createMetodoPagamentoService = createMetodoPagamentoService;
        _updateMetodoPagamentoService = updateMetodoPagamentoService;
        _deleteMetodoPagamentoService = deleteMetodoPagamentoService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMetodoPagamento()
    {
        try
        {
            var result = await _getAllMetodoPagamentoServices.ExecuteAsync();
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar métodos de pagamento");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdMetodoPagamento(Guid id)
    {
        try
        {
            var result = await _getByIdMetodoPagamentoService.ExecuteAsync(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao procurar método de pagamento");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    public async Task<IActionResult> CreateMetodoPagamento(MetodoPagamentoDto metodoPagamentoDto)
    {
        try
        {
            var result = await _createMetodoPagamentoService.ExecuteAsync(metodoPagamentoDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao cadastrar método de pagamento");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateMetodoPagamento(Guid id, MetodoPagamentoDto metodoPagamentoDto)
    {
        try
        {
            var result = await _updateMetodoPagamentoService.ExecuteAsync(id, metodoPagamentoDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atualizar método de pagamento");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [Authorize(Roles = "ADMIN")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMetodoPagamento(Guid id)
    {
        try
        {
            var result = await _deleteMetodoPagamentoService.ExecuteAsync(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao deletar método de pagamento");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }
}
