using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.UseCase.Categorias;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.UseCase.Categorias;
public class GetAllCategoriaServiceTests
{
    private readonly Mock<ICategoriaRepository> _repositoryMock;
    private readonly Mock<ILogger<GetAllCategoriaUseCase>> _loggerMock;
    private readonly GetAllCategoriaUseCase _service;

    public GetAllCategoriaServiceTests()
    {
        _repositoryMock = new Mock<ICategoriaRepository>();
        _loggerMock = new Mock<ILogger<GetAllCategoriaUseCase>>();
        _service = new GetAllCategoriaUseCase(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllCategorias_ReturnsNoCategorias_WhenNoCategoriasExist()
    {
        _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Categoria>());

        var result = await _service.ExecuteAsync();

        Assert.NotNull(result);
        Assert.Equal("Nenhuma categoria encontrada", result.Message);
        Assert.Equal(404, result.StatusCode);
        Assert.True(result.ReqSuccess);
        _repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllCategorias_ReturnsCategorias_WhenCategoriasExist()
    {
        var categorias = new List<Categoria>
        {
            new Categoria { Id = Guid.NewGuid(), Nome = "LANCHES" },
            new Categoria { Id = Guid.NewGuid(), Nome = "BEBIDAS" },
        };

        _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categorias);

        var result = await _service.ExecuteAsync();

        Assert.NotNull(result);
        Assert.True(result.ReqSuccess);
        Assert.Equal(categorias, result.Result);
        Assert.Equal(200, result.StatusCode);
        _repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllCategorias_LogsError_WhenExceptionIsThrown()
    {
        var exception = new Exception("Database Error");
        _repositoryMock.Setup(repo => repo.GetAllAsync()).ThrowsAsync(exception);

        var result = await _service.ExecuteAsync();

        Assert.False(result.ReqSuccess);
        Assert.Equal("Erro inesperado: Database Error", result.Message);
        Assert.Equal(500, result.StatusCode);
        _repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }
}
