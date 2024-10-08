﻿using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Adicionais;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdicionalController : ControllerBase
{
    private readonly IGetAllAdicionalUseCase _getAllAdicionalServices;
    private readonly IGetByIdAdicionalUseCase _getByIdAdicionalService;
    private readonly ICreateAdicionalUseCase _createAdicionalService;
    private readonly IUpdateAdicionalUseCase _updateAdicionalService;
    private readonly IDeleteAdicionalUseCase _deleteAdicionalService;
    private readonly ILogger<AdicionalController> _logger;

    public AdicionalController(
        ICreateAdicionalUseCase createAdicionalService,
        IGetAllAdicionalUseCase getAllAdicionalServices,
        IGetByIdAdicionalUseCase getByIdAdicionalService,
        IUpdateAdicionalUseCase updateAdicionalService,
        IDeleteAdicionalUseCase deleteAdicionalService,
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
    [HasPermission("Adicional", "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAdicionais([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _getAllAdicionalServices.ExecuteAsync(pageNumber, pageSize);
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
    [HasPermission("Adicional", "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdAdicional(Guid id)
    {
        try
        {
            var result = await _getByIdAdicionalService.ExecuteAsync(id);
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

    [HttpPost]
    [HasPermission("Adicional", "Create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAdicional(CreateAdicionalDto adicionalDto)
    {
        try
        {
            var result = await _createAdicionalService.ExecuteAsync(adicionalDto);
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

    [HttpPatch("{id:guid}")]
    [HasPermission("Adicional", "Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAdicional(Guid id, UpdateAdicionalDto adicionalDto)
    {
        try
        {
            var result = await _updateAdicionalService.ExecuteAsync(id, adicionalDto);
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

    [HttpDelete("{id:guid}")]
    [HasPermission("Adicional", "Delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAdicional(Guid id)
    {
        try
        {
            var result = await _deleteAdicionalService.ExecuteAsync(id);
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
