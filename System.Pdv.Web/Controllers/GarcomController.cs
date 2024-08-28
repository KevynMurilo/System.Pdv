using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Garcons;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class GarcomController : ControllerBase
{
    private readonly IGetAllGarcomService _getAllGarcomService;
    private readonly IGetByIdGarcomService _getByIdGarcomService;
    private readonly ICreateGarcomService _createGarcomService;
    private readonly ILogger<ClienteController> _logger;
    public GarcomController(
        IGetAllGarcomService getAllGarcomService,
        IGetByIdGarcomService getByIdGarcomService,
        ICreateGarcomService createGarcomService,
        ILogger<ClienteController> logger)
    {
        _getAllGarcomService = getAllGarcomService;
        _getByIdGarcomService = getByIdGarcomService;
        _createGarcomService = createGarcomService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGarcom()
    {
        try
        {
            var result = await _getAllGarcomService.GetAllGarcom();
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar garçons");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var result = await _getByIdGarcomService.GetById(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao procurar garçom por id");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddGarcom(RegisterGarcomDto garcomDto)
    {
        try
        {
            var result = await _createGarcomService.CreateGarcom(garcomDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao registrar garçom");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }
}
