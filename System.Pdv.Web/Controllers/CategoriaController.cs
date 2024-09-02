using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Categorias;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriaController : ControllerBase
{
    private readonly IGetAllCategoriaService _getAllCategoriaService;
    private readonly ICreateCategoriaService _createCategoriaService;
    private readonly IUpdateCategoriaService _updateCategoriaService;
    private readonly ILogger<CategoriaController> _logger;
    public CategoriaController(
        IGetAllCategoriaService getAllCategoriaService,
        ICreateCategoriaService createCategoriaService,
        IUpdateCategoriaService updateCategoriaService,
        ILogger<CategoriaController> logger)
    {
        _getAllCategoriaService = getAllCategoriaService;
        _createCategoriaService = createCategoriaService;
        _updateCategoriaService = updateCategoriaService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategorias()
    {
        try
        {
            var result = await _getAllCategoriaService.GetAllCategorias();
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

    [HttpPost]
    public async Task<IActionResult> CreateCategoria(CategoriaDto categoriaDto)
    {
        try
        {
            var result = await _createCategoriaService.CreateCategoria(categoriaDto);
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

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCategoria(Guid id, CategoriaDto categoriaDto)
    {
        try
        {
            var result = await _updateCategoriaService.UpdateCategoria(id, categoriaDto);
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
}
