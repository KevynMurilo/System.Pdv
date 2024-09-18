using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Categorias;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriaController : ControllerBase
{
    private readonly IGetAllCategoriaUseCase _getAllCategoriaService;
    private readonly IGetByIdCategoriaUseCase _getByIdCategoriaService;
    private readonly ICreateCategoriaUseCase _createCategoriaService;
    private readonly IUpdateCategoriaUseCase _updateCategoriaService;
    private readonly IDeleteCategoriaUseCase _deleteCategoriaService;
    private readonly ILogger<CategoriaController> _logger;
    public CategoriaController(
        IGetAllCategoriaUseCase getAllCategoriaService,
        IGetByIdCategoriaUseCase getByIdCategoriaService,
        ICreateCategoriaUseCase createCategoriaService,
        IUpdateCategoriaUseCase updateCategoriaService,
        IDeleteCategoriaUseCase deleteCategoriaService,
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
    [HasPermission("Categoria", "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
    [HasPermission("Categoria", "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    [HttpPost]
    [HasPermission("Categoria", "Create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    [HttpPut("{id:guid}")]
    [HasPermission("Categoria", "Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    [HttpDelete("{id:guid}")]
    [HasPermission("Categoria", "Delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
