using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Services.Produtos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.Services.Produtos;
public class UpdateProdutoServiceTests
{
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
    private readonly Mock<ICategoriaRepository> _categoriaRepositoryMock;
    private readonly Mock<ILogger<UpdateProdutoService>> _loggerMock;
    private readonly UpdateProdutoService _updateProdutoService;

    public UpdateProdutoServiceTests()
    {
        _produtoRepositoryMock = new Mock<IProdutoRepository>();
        _categoriaRepositoryMock = new Mock<ICategoriaRepository>();
        _loggerMock = new Mock<ILogger<UpdateProdutoService>>();
        _updateProdutoService = new UpdateProdutoService(_produtoRepositoryMock.Object, _categoriaRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task UpdateProduto_ShouldReturnUpdatedProduto_WhenProdutoExistsAndCategoriaIsValid()
    {
        var produtoId = Guid.NewGuid();
        var categoriaId = Guid.NewGuid();
        var produto = new Produto { Id = produtoId, Nome = "Produto Antigo", CategoriaId = categoriaId };
        var categoria = new Categoria { Id = categoriaId, Nome = "Categoria Atualizada" };
        var produtoDto = new ProdutoDto
        {
            Nome = "Produto Atualizado",
            Descricao = "Nova Descrição",
            Preco = 150.00m,
            Disponivel = true,
            CategoriaId = categoria.Id
        };

        _produtoRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoId)).ReturnsAsync(produto);
        _categoriaRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoDto.CategoriaId)).ReturnsAsync(categoria);
        _produtoRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Produto>())).Returns(Task.CompletedTask);

        var result = await _updateProdutoService.UpdateProduto(produtoId, produtoDto);

        Assert.NotNull(result.Result);
        Assert.Equal(produtoDto.Nome, result.Result.Nome);
        Assert.Equal(produtoDto.Descricao, result.Result.Descricao);
        Assert.Equal(produtoDto.Preco, result.Result.Preco);
        Assert.Equal(produtoDto.CategoriaId, result.Result.Categoria.Id);
    }

    [Fact]
    public async Task UpdateProduto_ShouldReturnError_WhenProdutoDoesNotExist()
    {
        var produtoId = Guid.NewGuid();
        var produtoDto = new ProdutoDto { Nome = "Produto Teste", CategoriaId = Guid.NewGuid() };
        _produtoRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoId)).ReturnsAsync((Produto)null);

        var result = await _updateProdutoService.UpdateProduto(produtoId, produtoDto);

        Assert.Null(result.Result);
        Assert.Equal("Produto não encontrado", result.Message);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task UpdateProduto_ShouldReturnError_WhenCategoriaDoesNotExist()
    {
        var produtoId = Guid.NewGuid();
        var produto = new Produto { Id = produtoId, Nome = "Produto Teste" };
        var produtoDto = new ProdutoDto { Nome = "Produto Atualizado", CategoriaId = Guid.NewGuid() };

        _produtoRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoId)).ReturnsAsync(produto);
        _categoriaRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoDto.CategoriaId)).ReturnsAsync((Categoria)null);

        var result = await _updateProdutoService.UpdateProduto(produtoId, produtoDto);

        Assert.Null(result.Result);
        Assert.Equal("Categoria não encontrada", result.Message);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task UpdateProduto_ShouldLogError_WhenExceptionOccurs()
    {
        var produtoId = Guid.NewGuid();
        var produtoDto = new ProdutoDto { Nome = "Produto Atualizado", CategoriaId = Guid.NewGuid() };

        _produtoRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoId)).ThrowsAsync(new Exception("Database error"));

        var result = await _updateProdutoService.UpdateProduto(produtoId, produtoDto);

        Assert.False(result.ServerOn);
        Assert.Equal("Erro inesperado: Database error", result.Message);
        Assert.Equal(500, result.StatusCode);
    }
}
