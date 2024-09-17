using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Mesas;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MesaController : ControllerBase
{
    private readonly IGetAllMesaUseCase _getAllServices;
    private readonly IGetMesaByIdUseCase _getMesaById;
    private readonly ICreateMesaUseCase _createMesaService;
    private readonly IUpdateMesaUseCase _updateMesaService;
    private readonly IDeleteMesaUseCase _deleteMesaService;
    private readonly ILogger<MesaController> _logger;
    public MesaController(
        IGetAllMesaUseCase getAllServices,
        IGetMesaByIdUseCase getMesaById,
        ICreateMesaUseCase createMesaService,
        IUpdateMesaUseCase updateMesaService,
        IDeleteMesaUseCase deleteMesaService,
        ILogger<MesaController> logger)
    {
        _getAllServices = getAllServices;
        _getMesaById = getMesaById;
        _createMesaService = createMesaService;
        _updateMesaService = updateMesaService;
        _deleteMesaService = deleteMesaService;
        _logger = logger;
    }

    [HasPermission("Mesa", "Get")]
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

    [HasPermission("Mesa", "Get")]
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

    [HasPermission("Mesa", "Create")]
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

    [HasPermission("Mesa", "Update")]
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

    [HasPermission("Mesa", "Delete")]
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
