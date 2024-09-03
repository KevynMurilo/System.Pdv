using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Pdv.Application.Services.Produtos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.Services.Produtos;
public class GetProdutoByCategoriaServiceTests
{
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
    private readonly Mock<ILogger<GetProdutoByCategoriaService>> _loggerMock;
    private readonly GetProdutoByCategoriaService _getProdutoByCategoriaService;

    public GetProdutoByCategoriaServiceTests()
    {
        _produtoRepositoryMock = new Mock<IProdutoRepository>();
        _loggerMock = new Mock<ILogger<GetProdutoByCategoriaService>>();
        _getProdutoByCategoriaService = new GetProdutoByCategoriaService(_produtoRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetProdutoByCategoria_ShouldReturnProdutos_WhenProdutosExistForCategoria()
    {
        var categoriaId = Guid.NewGuid();
        var produtos = new List<Produto>
        {
            new Produto { Nome = "Produto 1", CategoriaId = categoriaId, Preco = 10.0m },
            new Produto { Nome = "Produto 2", CategoriaId = categoriaId, Preco = 20.0m }
        };

        _produtoRepositoryMock.Setup(repo => repo.GetProdutoByCategoria(categoriaId, 1, 10)).ReturnsAsync(produtos);

        var result = await _getProdutoByCategoriaService.GetProdutoByCategoria(categoriaId, 1, 10);

        Assert.NotNull(result.Result);
        Assert.Equal(2, result.Result.Count());
        Assert.Equal("Produto 1", result.Result.First().Nome);
        Assert.Equal(categoriaId, result.Result.First().CategoriaId);
    }

    [Fact]
    public async Task GetProdutoByCategoria_ShouldReturnError_WhenNoProdutosFoundForCategoria()
    {
        var categoriaId = Guid.NewGuid();
        _produtoRepositoryMock.Setup(repo => repo.GetProdutoByCategoria(categoriaId, 1, 10)).ReturnsAsync(new List<Produto>());

        var result = await _getProdutoByCategoriaService.GetProdutoByCategoria(categoriaId, 1, 10);

        Assert.Null(result.Result);
        Assert.Equal("Nenhum produto encontrado para a categoria especificada", result.Message);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task GetProdutoByCategoria_ShouldLogError_WhenExceptionOccurs()
    {
        var categoriaId = Guid.NewGuid();
        _produtoRepositoryMock.Setup(repo => repo.GetProdutoByCategoria(categoriaId, 1, 10)).ThrowsAsync(new Exception("Database error"));

        var result = await _getProdutoByCategoriaService.GetProdutoByCategoria(categoriaId, 1, 10);

        Assert.False(result.ServerOn);
        Assert.Equal("Erro inesperado: Database error", result.Message);
        Assert.Equal(500, result.StatusCode);
    }
}
