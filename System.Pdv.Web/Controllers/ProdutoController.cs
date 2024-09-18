using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Produtos;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly IGetAllProdutoUseCase _getAllProdutoService;
    private readonly IGetByIdProdutoUseCase _getByIdProdutoService;
    private readonly IGetProdutoByCategoriaUseCase _getProdutoByCategoriaService;
    private readonly ICreateProdutoUseCase _createProdutoService;
    private readonly IUpdateProdutoUseCase _updateProdutoService;
    private readonly IDeleteProdutoUseCase _deleteProdutoService;
    private readonly ILogger<ProdutoController> _logger;

    public ProdutoController(
        IGetAllProdutoUseCase getAllProdutoService,
        IGetByIdProdutoUseCase getByIdProdutoService,
        IGetProdutoByCategoriaUseCase getProdutoByCategoriaService,
        ICreateProdutoUseCase createProdutoService,
        IUpdateProdutoUseCase updateProdutoService,
        IDeleteProdutoUseCase deleteProdutoService,
        ILogger<ProdutoController> logger)
    {
        _getAllProdutoService = getAllProdutoService;
        _getByIdProdutoService = getByIdProdutoService;
        _getProdutoByCategoriaService = getProdutoByCategoriaService;
        _createProdutoService = createProdutoService;
        _updateProdutoService = updateProdutoService;
        _deleteProdutoService = deleteProdutoService;
        _logger = logger;
    }

    [HttpGet] 
    [HasPermission("Produto", "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllProdutos([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _getAllProdutoService.ExecuteAsync(pageNumber, pageSize);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar produtos");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpGet("{id:guid}")]
    [HasPermission("Produto", "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProdutoById(Guid id)
    {
        try
        {
            var result = await _getByIdProdutoService.ExecuteAsync(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar produto por id");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpGet("categoria/{categoriaId:guid}")]
    [HasPermission("Produto", "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProdutosByCategoria(Guid categoriaId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _getProdutoByCategoriaService.ExecuteAsync(categoriaId, pageNumber, pageSize);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar produtos por categoria");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
        
    }

    [HttpPost]
    [HasPermission("Produto", "Create")]
    [ProducesResponseType(StatusCodes.Status200OK)]  
    [ProducesResponseType(StatusCodes.Status404NotFound)]  
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProduto(ProdutoDto produtoDto)
    {
        try
        {
            var result = await _createProdutoService.ExecuteAsync(produtoDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao registrar produto");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpPatch("{id:guid}")]
    [HasPermission("Produto", "Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProduto(Guid id, ProdutoDto produtoDto)
    {
        try
        {
            var result = await _updateProdutoService.ExecuteAsync(id, produtoDto);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atualizar produto");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }

    [HttpDelete("{id:guid}")]
    [HasPermission("Produto", "Delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProduto(Guid id)
    {
        try
        {
            var result = await _deleteProdutoService.ExecuteAsync(id);
            return result.StatusCode == 200
                ? Ok(result)
                : StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao deletar produto");
            return StatusCode(500, "Ocorreu um erro inesperado.");
        }
    }
}
