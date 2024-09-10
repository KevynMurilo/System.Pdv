using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.UseCase.Categorias;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.UseCase.Categorias;
public class GetByIdCategoriaServiceTests
{
    private readonly Mock<ICategoriaRepository> _repositoryMock;
    private readonly Mock<ILogger<GetByIdCategoriaUseCase>> _loggerMock;
    private readonly GetByIdCategoriaUseCase _service;

    public GetByIdCategoriaServiceTests()
    {
        _repositoryMock = new Mock<ICategoriaRepository>();
        _loggerMock = new Mock<ILogger<GetByIdCategoriaUseCase>>();
        _service = new GetByIdCategoriaUseCase(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetById_CategoriaExists_ReturnsCategoria()
    {
        var categoriaId = Guid.NewGuid();
        var categoria = new Categoria { Id = categoriaId };

        _repositoryMock.Setup(repo => repo.GetByIdAsync(categoriaId))
            .ReturnsAsync(categoria);

        var result = await _service.ExecuteAsync(categoriaId);

        Assert.NotNull(result);
        Assert.Equal(categoria, result.Result);
        Assert.Equal(200, result.StatusCode);
        Assert.True(result.ServerOn);
        _repositoryMock.Verify(repo => repo.GetByIdAsync(categoriaId), Times.Once);
    }

    [Fact]
    public async Task GetById_CategoriaNotFound_ReturnsNotFound()
    {
        var categoriaId = Guid.NewGuid();
        _repositoryMock.Setup(repo => repo.GetByIdAsync(categoriaId))
            .ReturnsAsync((Categoria)null);

        var result = await _service.ExecuteAsync(categoriaId);

        Assert.NotNull(result);
        Assert.Null(result.Result);
        Assert.Equal("Categoria não encontrada", result.Message);
        Assert.Equal(404, result.StatusCode);
        Assert.True(result.ServerOn);
        _repositoryMock.Verify(repo => repo.GetByIdAsync(categoriaId), Times.Once);
    }

    [Fact]
    public async Task GetById_ExceptionThrown_LogsErrorAndReturnsServerError()
    {
        var categoriaId = Guid.NewGuid();
        var exception = new Exception("Database failure");

        _repositoryMock.Setup(repo => repo.GetByIdAsync(categoriaId))
            .ThrowsAsync(exception);

        var result = await _service.ExecuteAsync(categoriaId);

        Assert.NotNull(result);
        Assert.Null(result.Result);
        Assert.Equal("Erro inesperado: Database failure", result.Message);
        Assert.Equal(500, result.StatusCode);
        _repositoryMock.Verify(repo => repo.GetByIdAsync(categoriaId), Times.Once);
    }
}
