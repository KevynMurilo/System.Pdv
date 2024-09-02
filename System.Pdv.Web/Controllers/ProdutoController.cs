using Microsoft.AspNetCore.Mvc;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Produtos;

namespace System.Pdv.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly IGetAllProdutoService _getAllProdutoService;
    private readonly IGetByIdProdutoService _getByIdProdutoService;
    private readonly IGetProdutoByCategoriaService _getProdutoByCategoriaService;
    private readonly ICreateProdutoService _createProdutoService;
    private readonly IUpdateProdutoService _updateProdutoService;
    private readonly ILogger<ProdutoController> _logger;

    public ProdutoController(
        IGetAllProdutoService getAllProdutoService,
        IGetByIdProdutoService getByIdProdutoService,
        IGetProdutoByCategoriaService getProdutoByCategoriaService,
        ICreateProdutoService createProdutoService,
        IUpdateProdutoService updateProdutoService,
        ILogger<ProdutoController> logger)
    {
        _getAllProdutoService = getAllProdutoService;
        _getByIdProdutoService = getByIdProdutoService;
        _getProdutoByCategoriaService = getProdutoByCategoriaService;
        _createProdutoService = createProdutoService;
        _updateProdutoService = updateProdutoService;
        _logger = logger;
    }

    [HttpGet] 
    public async Task<IActionResult> GetAllProdutos([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _getAllProdutoService.GetAllProduto(pageNumber, pageSize);
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
    public async Task<IActionResult> GetProdutoById(Guid id)
    {
        try
        {
            var result = await _getByIdProdutoService.GetByIdProduto(id);
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
    public async Task<IActionResult> GetProdutosByCategoria(Guid categoriaId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _getProdutoByCategoriaService.GetProdutoByCategoria(categoriaId, pageNumber, pageSize);
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
    public async Task<IActionResult> CreateProduto(ProdutoDto produtoDto)
    {
        try
        {
            var result = await _createProdutoService.CreateProduto(produtoDto);
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
    public async Task<IActionResult> UpdateProduto(Guid id, ProdutoDto produtoDto)
    {
        try
        {
            var result = await _updateProdutoService.UpdateProduto(id, produtoDto);
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
}
