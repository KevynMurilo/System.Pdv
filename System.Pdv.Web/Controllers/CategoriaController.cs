using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Categorias;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriaController : ControllerBase
{
    private readonly IGetAllCategoriaService _getAllCategoriaService;
    private readonly IGetByIdCategoriaService _getByIdCategoriaService;
    private readonly ICreateCategoriaService _createCategoriaService;
    private readonly IUpdateCategoriaService _updateCategoriaService;
    private readonly IDeleteCategoriaService _deleteCategoriaService;
    private readonly ILogger<CategoriaController> _logger;
    public CategoriaController(
        IGetAllCategoriaService getAllCategoriaService,
        IGetByIdCategoriaService getByIdCategoriaService,
        ICreateCategoriaService createCategoriaService,
        IUpdateCategoriaService updateCategoriaService,
        IDeleteCategoriaService deleteCategoriaService,
        ILogger<CategoriaController> logger)
    {
        _getAllCategoriaService = getAllCategoriaService;
        _getByIdCategoriaService= getByIdCategoriaService;
        _createCategoriaService = createCategoriaService;
        _updateCategoriaService = updateCategoriaService;
        _deleteCategoriaService = deleteCategoriaService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategorias()
    {
        try
        {
            var result = await _getAllCategoriaService.ExecuteAsync();
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar categorias");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdCategoria(Guid id)
    {
        try
        {
            var result = await _getByIdCategoriaService.ExecuteAsync(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao procurar categoria por id");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    public async Task<IActionResult> CreateCategoria(CategoriaDto categoriaDto)
    {
        try
        {
            var result = await _createCategoriaService.ExecuteAsync(categoriaDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao cadastrar categoria");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCategoria(Guid id, CategoriaDto categoriaDto)
    {
        try
        {
            var result = await _updateCategoriaService.ExecuteAsync(id, categoriaDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atualizar categoria");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [Authorize(Roles = "ADMIN")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategoria(Guid id)
    {
        try
        {
            var result = await _deleteCategoriaService.ExecuteAsync(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao deletar categoria");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }
}
