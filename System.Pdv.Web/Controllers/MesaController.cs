using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Mesas;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class MesaController : ControllerBase
{
    private readonly IGetAllServices _getAllServices;
    private readonly IGetMesaByIdService _getMesaById;
    private readonly ICreateMesaService _createMesaService;
    private readonly IUpdateMesaService _updateMesaService;
    private readonly IDeleteMesaService _deleteMesaService;
    private readonly ILogger<MesaController> _logger;
    public MesaController(
        IGetAllServices getAllServices,
        IGetMesaByIdService getMesaById,
        ICreateMesaService createMesaService,
        IUpdateMesaService updateMesaService,
        IDeleteMesaService deleteMesaService,
        ILogger<MesaController> logger)
    {
        _getAllServices = getAllServices;
        _getMesaById = getMesaById;
        _createMesaService = createMesaService;
        _updateMesaService = updateMesaService;
        _deleteMesaService = deleteMesaService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMesas()
    {
        try
        {
            var result = await _getAllServices.ExecuteAsync();
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar mesas");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var result = await _getMesaById.ExecuteAsync(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar mesas");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    public async Task<IActionResult> CreateMesa(MesaDto mesaDto)
    {
        try
        {
            var result = await _createMesaService.ExecuteAsync(mesaDto);
            return result.StatusCode == 200
               ? Ok(result)
               : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao cadastrar uma mesa");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateMesa(Guid id, MesaDto mesaDto)
    {
        try
        {
            var result = await _updateMesaService.ExecuteAsync(id, mesaDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar mesas");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [Authorize(Roles = "ADMIN")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMesa(Guid id)
    {
        try
        {
            var result = await _deleteMesaService.ExecuteAsync(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao deletar mesa");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }
}
