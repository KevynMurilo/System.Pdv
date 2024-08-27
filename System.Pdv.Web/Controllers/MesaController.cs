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
    private readonly ILogger<MesaController> _logger;
    public MesaController(
        ICreateMesaService createMesaService,
        IGetAllServices getAllServices,
        ILogger<MesaController> logger)
    {
        _createMesaService = createMesaService;
        _getAllServices = getAllServices;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMesa(MesaDto mesaDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var mesa = await _createMesaService.CreateMesa(mesaDto);
            if (mesa.StatusCode == 409) return StatusCode(409, mesa.Message);

            return Ok(mesa);
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
            var mesa = await _getAllServices.GetAllMesas();
            if (mesa.StatusCode == 404) return StatusCode(404, mesa.Message);
            return Ok(mesa);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar mesas");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }
}
