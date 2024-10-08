﻿using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Pdv.Application.UseCase.Produtos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.UseCase.Produtos;
public class GetByIdProdutoServiceTests
{
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
    private readonly Mock<ILogger<GetByIdProdutoUseCase>> _loggerMock;
    private readonly GetByIdProdutoUseCase _getByIdProdutoService;

    public GetByIdProdutoServiceTests()
    {
        _produtoRepositoryMock = new Mock<IProdutoRepository>();
        _loggerMock = new Mock<ILogger<GetByIdProdutoUseCase>>();
        _getByIdProdutoService = new GetByIdProdutoUseCase(_produtoRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetByIdProduto_ShouldReturnProduto_WhenProdutoExists()
    {
        var produtoId = Guid.NewGuid();
        var produto = new Produto { Id = produtoId, Nome = "Produto Teste", Preco = 100.0m };

        _produtoRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoId)).ReturnsAsync(produto);

        var result = await _getByIdProdutoService.ExecuteAsync(produtoId);

        Assert.NotNull(result.Result);
        Assert.Equal(produto, result.Result);
        Assert.Equal(produtoId, result.Result.Id);
        _produtoRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdProduto_ShouldReturnError_WhenProdutoDoesNotExist()
    {
        var produtoId = Guid.NewGuid();
        _produtoRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoId)).ReturnsAsync((Produto)null);

        var result = await _getByIdProdutoService.ExecuteAsync(produtoId);

        Assert.Null(result.Result);
        Assert.Equal("Produto não encontrado", result.Message);
        Assert.Equal(404, result.StatusCode);
        _produtoRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdProduto_ShouldLogError_WhenExceptionOccurs()
    {
        var produtoId = Guid.NewGuid();
        _produtoRepositoryMock.Setup(repo => repo.GetByIdAsync(produtoId)).ThrowsAsync(new Exception("Database error"));

        var result = await _getByIdProdutoService.ExecuteAsync(produtoId);

        Assert.False(result.ReqSuccess);
        Assert.Equal("Erro inesperado: Database error", result.Message);
        Assert.Equal(500, result.StatusCode);
        _produtoRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}
