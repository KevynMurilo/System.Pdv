using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Pdv.Application.Services.Produtos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.Services.Produtos;
public class GetByIdProdutoServiceTests
{
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
    private readonly Mock<ILogger<GetByIdProdutoService>> _loggerMock;
    private readonly GetByIdProdutoService _getByIdProdutoService;

    public GetByIdProdutoServiceTests()
    {
        _produtoRepositoryMock = new Mock<IProdutoRepository>();
        _loggerMock = new Mock<ILogger<GetByIdProdutoService>>();
        _getByIdProdutoService = new GetByIdProdutoService(_produtoRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetByIdProduto_ShouldReturnProduto_WhenProdutoExists()
    {
        var produtoId = Guid.NewGuid();
        var produto = new Produto { Id = produtoId, Nome = "Produto Teste", Preco = 100.0m };

        _produtoRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoId)).ReturnsAsync(produto);

        var result = await _getByIdProdutoService.GetByIdProduto(produtoId);

        Assert.NotNull(result.Result);
        Assert.Equal(produto, result.Result);
        Assert.Equal(produtoId, result.Result.Id);
    }

    [Fact]
    public async Task GetByIdProduto_ShouldReturnError_WhenProdutoDoesNotExist()
    {
        var produtoId = Guid.NewGuid();
        _produtoRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoId)).ReturnsAsync((Produto)null);

        var result = await _getByIdProdutoService.GetByIdProduto(produtoId);

        Assert.Null(result.Result);
        Assert.Equal("Produto não encontrado", result.Message);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task GetByIdProduto_ShouldLogError_WhenExceptionOccurs()
    {
        var produtoId = Guid.NewGuid();
        _produtoRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoId)).ThrowsAsync(new Exception("Database error"));

        var result = await _getByIdProdutoService.GetByIdProduto(produtoId);

        Assert.False(result.ServerOn);
        Assert.Equal("Erro inesperado: Database error", result.Message);
        Assert.Equal(500, result.StatusCode);
    }
}
