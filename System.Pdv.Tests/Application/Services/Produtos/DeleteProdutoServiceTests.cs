using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Pdv.Application.Services.Produtos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.Services.Produtos;
public class DeleteProdutoServiceTests
{
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
    private readonly Mock<ILogger<DeleteProdutoService>> _loggerMock;
    private readonly DeleteProdutoService _deleteProdutoService;

    public DeleteProdutoServiceTests()
    {
        _produtoRepositoryMock = new Mock<IProdutoRepository>();
        _loggerMock = new Mock<ILogger<DeleteProdutoService>>();
        _deleteProdutoService = new DeleteProdutoService(_produtoRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task DeleteProduto_ShouldReturnProduto_WhenProdutoExists()
    {
        var produtoId = Guid.NewGuid();
        var produto = new Produto { Id = produtoId, Nome = "Produto Teste" };

        _produtoRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoId)).ReturnsAsync(produto);
        _produtoRepositoryMock.Setup(repo => repo.DeleteAsync(produto)).Returns(Task.CompletedTask);

        var result = await _deleteProdutoService.DeleteProduto(produtoId);

        Assert.NotNull(result.Result);
        Assert.Equal(produto, result.Result);
        Assert.Equal("Produto deletado com sucesso", result.Message);
    }

    [Fact]
    public async Task DeleteProduto_ShouldReturnError_WhenProdutoDoesNotExist()
    {
        var produtoId = Guid.NewGuid();

        _produtoRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoId)).ReturnsAsync((Produto)null);

        var result = await _deleteProdutoService.DeleteProduto(produtoId);

        Assert.Null(result.Result);
        Assert.Equal("Produto não encontrado", result.Message);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task DeleteProduto_ShouldLogError_WhenExceptionOccurs()
    {
        var produtoId = Guid.NewGuid();

        _produtoRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoId)).ThrowsAsync(new Exception("Database error"));

        var result = await _deleteProdutoService.DeleteProduto(produtoId);

        Assert.False(result.ServerOn);
        Assert.Equal("Erro inesperado: Database error", result.Message);
        Assert.Equal(500, result.StatusCode);
    }
}
