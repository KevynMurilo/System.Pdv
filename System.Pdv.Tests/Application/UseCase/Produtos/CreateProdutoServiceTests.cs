using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.UseCase.Produtos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.UseCase.Produtos;
public class CreateProdutoServiceTests
{
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
    private readonly Mock<ICategoriaRepository> _categoriaRepositoryMock;
    private readonly Mock<ILogger<CreateProdutoUseCase>> _loggerMock;
    private readonly CreateProdutoUseCase _createProdutoService;

    public CreateProdutoServiceTests()
    {
        _produtoRepositoryMock = new Mock<IProdutoRepository>();
        _categoriaRepositoryMock = new Mock<ICategoriaRepository>();
        _loggerMock = new Mock<ILogger<CreateProdutoUseCase>>();
        _createProdutoService = new CreateProdutoUseCase(_produtoRepositoryMock.Object, _categoriaRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task CreateProduto_ShouldReturnProduto_WhenCategoriaExists()
    {
        var produtoDto = new CreateProdutoDto
        {
            Nome = "Produto Teste",
            Descricao = "Descrição do produto teste",
            Preco = 20.50m,
            Disponivel = true,
            CategoriaId = Guid.NewGuid()
        };

        var categoria = new Categoria { Id = produtoDto.CategoriaId, Nome = "Categoria Teste" };
        var produtoAdicionado = new Produto { Id = Guid.NewGuid(), Nome = produtoDto.Nome, Descricao = produtoDto.Descricao, Preco = produtoDto.Preco, Disponivel = produtoDto.Disponivel, CategoriaId = produtoDto.CategoriaId };

        _categoriaRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoDto.CategoriaId)).ReturnsAsync(categoria);

        _produtoRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Produto>())).Returns(Task.CompletedTask);
        _produtoRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(produtoAdicionado);

        var result = await _createProdutoService.ExecuteAsync(produtoDto);

        Assert.NotNull(result.Result);
        Assert.Equal(produtoDto.Nome, result.Result.Nome);
        Assert.Equal(produtoDto.Descricao, result.Result.Descricao);
        Assert.Equal(produtoDto.Preco, result.Result.Preco);
        Assert.Equal(produtoDto.Disponivel, result.Result.Disponivel);
        Assert.Equal(produtoDto.CategoriaId, result.Result.CategoriaId);
        _produtoRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Produto>()), Times.Once);
        _categoriaRepositoryMock.Verify(repo => repo.GetByIdAsync(produtoDto.CategoriaId), Times.Once);
    }

    [Fact]
    public async Task CreateProduto_ShouldReturnError_WhenCategoriaDoesNotExist()
    {
        var produtoDto = new CreateProdutoDto
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
        var produtoDto = new CreateProdutoDto
        {
            Nome = "Produto Teste",
            Descricao = "Descrição do produto teste",
            Preco = 100.0m,
            Disponivel = true,
            CategoriaId = Guid.NewGuid()
        };

        _categoriaRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoDto.CategoriaId)).ThrowsAsync(new Exception("Database error"));

        var result = await _createProdutoService.ExecuteAsync(produtoDto);

        Assert.False(result.ReqSuccess);
        Assert.Equal("Erro inesperado: Database error", result.Message);
        Assert.Equal(500, result.StatusCode);
        _produtoRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Produto>()), Times.Never);
        _categoriaRepositoryMock.Verify(repo => repo.GetByIdAsync(produtoDto.CategoriaId), Times.Once);
    }
}
