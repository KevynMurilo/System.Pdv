﻿using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    [HasPermission("Mesa", "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
    [HasPermission("Mesa", "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    [HttpPost]
    [HasPermission("Mesa", "Create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateMesa(CreateMesaDto mesaDto)
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

    [HttpPatch("{id:guid}")]
    [HasPermission("Mesa", "Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateMesa(Guid id, UpdateMesaDto mesaDto)
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

    [HttpDelete("{id:guid}")]
    [HasPermission("Mesa", "Delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
