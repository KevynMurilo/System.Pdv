using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Services.Produtos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.Services.Produtos;
public class CreateProdutoServiceTests
{
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
    private readonly Mock<ICategoriaRepository> _categoriaRepositoryMock;
    private readonly Mock<ILogger<CreateProdutoService>> _loggerMock;
    private readonly CreateProdutoService _createProdutoService;

    public CreateProdutoServiceTests()
    {
        _produtoRepositoryMock = new Mock<IProdutoRepository>();
        _categoriaRepositoryMock = new Mock<ICategoriaRepository>();
        _loggerMock = new Mock<ILogger<CreateProdutoService>>();
        _createProdutoService = new CreateProdutoService(_produtoRepositoryMock.Object, _categoriaRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task CreateProduto_ShouldReturnProduto_WhenCategoriaExists()
    {
        var produtoDto = new ProdutoDto
        {
            Nome = "Produto Teste",
            Descricao = "Descrição do produto teste",
            Preco = 20.50m,
            Disponivel = true,
            CategoriaId = Guid.NewGuid()
        };

        var categoria = new Categoria { Id = Guid.NewGuid(), Nome = "Categoria Teste" };
        _categoriaRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoDto.CategoriaId)).ReturnsAsync(categoria);

        Produto? capturedProduto = null;
        _produtoRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Produto>()))
            .Callback<Produto>(produto => capturedProduto = produto)
            .Returns(Task.CompletedTask);

        var result = await _createProdutoService.ExecuteAsync(produtoDto);

        Assert.NotNull(result.Result);
        Assert.Equal(produtoDto.Nome, capturedProduto?.Nome);
        Assert.Equal(produtoDto.Descricao, capturedProduto?.Descricao);
        Assert.Equal(produtoDto.Preco, capturedProduto?.Preco);
        Assert.Equal(produtoDto.Disponivel, capturedProduto?.Disponivel);
        Assert.Equal(produtoDto.CategoriaId, capturedProduto?.CategoriaId);
        _produtoRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Produto>()), Times.Once);
        _categoriaRepositoryMock.Verify(repo => repo.GetByIdAsync(produtoDto.CategoriaId), Times.Once);
    }

    [Fact]
    public async Task CreateProduto_ShouldReturnError_WhenCategoriaDoesNotExist()
    {
        var produtoDto = new ProdutoDto
        {
            Nome = "Produto Teste",
            Descricao = "Descrição do produto teste",
            Preco = 100.0m,
            Disponivel = true,
            CategoriaId = Guid.NewGuid()
        };

        _categoriaRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoDto.CategoriaId)).ReturnsAsync((Categoria)null);

        var result = await _createProdutoService.ExecuteAsync(produtoDto);

        Assert.Null(result.Result);
        Assert.Equal("Categoria não encontrada", result.Message);
        Assert.Equal(404, result.StatusCode);
        _produtoRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Produto>()), Times.Never);
        _categoriaRepositoryMock.Verify(repo => repo.GetByIdAsync(produtoDto.CategoriaId), Times.Once);
    }

    [Fact]
    public async Task CreateProduto_ShouldLogError_WhenExceptionOccurs()
    {
        var produtoDto = new ProdutoDto
        {
            Nome = "Produto Teste",
            Descricao = "Descrição do produto teste",
            Preco = 100.0m,
            Disponivel = true,
            CategoriaId = Guid.NewGuid()
        };

        _categoriaRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoDto.CategoriaId)).ThrowsAsync(new Exception("Database error"));

        var result = await _createProdutoService.ExecuteAsync(produtoDto);

        Assert.False(result.ServerOn);
        Assert.Equal("Erro inesperado: Database error", result.Message);
        Assert.Equal(500, result.StatusCode);
        _produtoRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Produto>()), Times.Never);
        _categoriaRepositoryMock.Verify(repo => repo.GetByIdAsync(produtoDto.CategoriaId), Times.Once);
    }
}
