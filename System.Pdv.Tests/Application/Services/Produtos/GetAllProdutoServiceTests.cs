using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Pdv.Application.Services.Produtos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.Services.Produtos;
public class GetAllProdutoServiceTests
{
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
    private readonly Mock<ILogger<GetAllProdutoService>> _loggerMock;
    private readonly GetAllProdutoService _getAllProdutoService;

    public GetAllProdutoServiceTests()
    {
        _produtoRepositoryMock = new Mock<IProdutoRepository>();
        _loggerMock = new Mock<ILogger<GetAllProdutoService>>();
        _getAllProdutoService = new GetAllProdutoService(_produtoRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllProduto_ShouldReturnProdutos_WhenProdutosExist()
    {
        var produtos = new List<Produto>
        {
            new Produto { Nome = "Produto 1", Preco = 10.0m },
            new Produto { Nome = "Produto 2", Preco = 20.0m }
        };

        _produtoRepositoryMock.Setup(repo => repo.GetAllAsync(1, 10)).ReturnsAsync(produtos);

        var result = await _getAllProdutoService.GetAllProduto(1, 10);

        Assert.NotNull(result.Result);
        Assert.Equal(2, result.Result.Count());
        Assert.Equal("Produto 1", result.Result.First().Nome);
        Assert.Equal(10.0m, result.Result.First().Preco);
    }

    [Fact]
    public async Task GetAllProduto_ShouldReturnError_WhenNoProdutosFound()
    {
        _produtoRepositoryMock.Setup(repo => repo.GetAllAsync(1, 10)).ReturnsAsync(new List<Produto>());

        var result = await _getAllProdutoService.GetAllProduto(1, 10);

        Assert.Null(result.Result);
        Assert.Equal("Nenhum produto encontrado", result.Message);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task GetAllProduto_ShouldLogError_WhenExceptionOccurs()
    {
        _produtoRepositoryMock.Setup(repo => repo.GetAllAsync(1, 10)).ThrowsAsync(new Exception("Database error"));

        var result = await _getAllProdutoService.GetAllProduto(1, 10);

        Assert.False(result.ServerOn);
        Assert.Equal("Erro inesperado: Database error", result.Message);
        Assert.Equal(500, result.StatusCode);
    }
}
