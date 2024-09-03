using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Adicionais;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdicionalController : ControllerBase
{
    private readonly IGetAllAdicionalServices _getAllAdicionalServices;
    private readonly IGetByIdAdicionalService _getByIdAdicionalService;
    private readonly ICreateAdicionalService _createAdicionalService;
    private readonly IUpdateAdicionalService _updateAdicionalService;
    private readonly IDeleteAdicionalService _deleteAdicionalService;
    private readonly ILogger<AdicionalController> _logger;

    public AdicionalController(
        ICreateAdicionalService createAdicionalService,
        IGetAllAdicionalServices getAllAdicionalServices,
        IGetByIdAdicionalService getByIdAdicionalService,
        IUpdateAdicionalService updateAdicionalService,
        IDeleteAdicionalService deleteAdicionalService,
        ILogger<AdicionalController> logger)
    {
        _getAllAdicionalServices = getAllAdicionalServices;
        _getByIdAdicionalService = getByIdAdicionalService;
        _createAdicionalService = createAdicionalService;
        _updateAdicionalService = updateAdicionalService;   
        _deleteAdicionalService = deleteAdicionalService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAdicionais([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _getAllAdicionalServices.GetAllAdicionais(pageNumber, pageSize);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar adicionais");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAdicional(Guid id)
    {
        try
        {
            var result = await _getByIdAdicionalService.GetById(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao procurar adicional por id");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    public async Task<IActionResult> CreateAdicional(AdicionalDto adicionalDto)
    {
        try
        {
            var result = await _createAdicionalService.CreateAdicional(adicionalDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao cadastrar adicional");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateAdicional(Guid id, AdicionalDto adicionalDto)
    {
        try
        {
            var result = await _updateAdicionalService.UpdateAdicional(id, adicionalDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atualizar adicional");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [Authorize(Roles = "ADMIN")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAdicional(Guid id)
    {
        try
        {
            var result = await _deleteAdicionalService.DeleteAdicional(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao deletar adicional");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }
}
