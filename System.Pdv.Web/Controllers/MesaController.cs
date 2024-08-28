using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Clientes;
using System.Pdv.Application.Interfaces.Mesas;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class MesaController : ControllerBase
{
    private readonly ICreateMesaService _createMesaService;
    private readonly IGetAllServices _getAllServices;
    private readonly IUpdateMesaService _updateMesaService;
    private readonly IDeleteMesaService _deleteMesaService;
    private readonly ILogger<MesaController> _logger;
    public MesaController(
        ICreateMesaService createMesaService,
        IGetAllServices getAllServices,
        IUpdateMesaService updateMesaService,
        IDeleteMesaService deleteMesaService,
        ILogger<MesaController> logger)
    {
        _createMesaService = createMesaService;
        _getAllServices = getAllServices;
        _updateMesaService = updateMesaService;
        _deleteMesaService = deleteMesaService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMesa(MesaDto mesaDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _createMesaService.CreateMesa(mesaDto);
            return result.StatusCode == 200
               ? Ok(result)
               : StatusCode(result.StatusCode, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao cadastrar uma mesa");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMesas()
    {
        try
        {
            var result = await _getAllServices.GetAllMesas();
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar mesas");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateMesa(Guid id, MesaDto mesaDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _updateMesaService.UpdateMesa(id, mesaDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar mesas");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMesa(Guid id)
    {
        try
        {
            var result = await _deleteMesaService.DeleteMesa(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao deletar mesa");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }
}
