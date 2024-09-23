using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.UseCase.Categorias;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.UseCase.Categorias;
public class DeleteCategoriaServiceTests
{
    private readonly Mock<ICategoriaRepository> _repositoryMock;
    private readonly Mock<ILogger<DeleteCategoriaUseCase>> _loggerMock;
    private readonly DeleteCategoriaUseCase _service;

    public DeleteCategoriaServiceTests()
    {
        _repositoryMock = new Mock<ICategoriaRepository>();
        _loggerMock = new Mock<ILogger<DeleteCategoriaUseCase>>();
        _service = new DeleteCategoriaUseCase(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task DeleteCategoria_ShouldReturnNotFound_WhenCategoriaDoesNotExist()
    {
        var categoriaId = Guid.NewGuid();
        _repositoryMock.Setup(repo => repo.GetByIdAsync(categoriaId))
            .ReturnsAsync((Categoria)null);

        var result = await _service.ExecuteAsync(categoriaId);

        Assert.NotNull(result);
        Assert.True(result.ReqSuccess);
        Assert.Equal("Categoria não encontrada", result.Message);
        Assert.Equal(404, result.StatusCode);
        _repositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _repositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Categoria>()), Times.Never);
    }

    [Fact]
    public async Task DeleteCategoria_ShouldDeleteCategoria_WhenCategoriaExists()
    {
        var categoriaId = Guid.NewGuid();
        var categoria = new Categoria { Id = categoriaId };

        _repositoryMock.Setup(repo => repo.GetByIdAsync(categoriaId))
            .ReturnsAsync(categoria);

        var result = await _service.ExecuteAsync(categoriaId);

        _repositoryMock.Verify(repo => repo.DeleteAsync(categoria), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.True(result.ReqSuccess);
        Assert.Equal("Categoria deletada com sucesso!", result.Message);
        _repositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _repositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Categoria>()), Times.Once);
    }

    [Fact]
    public async Task DeleteCategoria_ShouldLogError_WhenExceptionIsThrown()
    {
        var categoriaId = Guid.NewGuid();
        var exception = new Exception("Database error");

        _repositoryMock.Setup(repo => repo.GetByIdAsync(categoriaId))
            .ThrowsAsync(exception);

        var result = await _service.ExecuteAsync(categoriaId);

        Assert.False(result.ReqSuccess);
        Assert.Equal("Erro inesperado: Database error", result.Message);
        Assert.Equal(500, result.StatusCode);
        _repositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _repositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Categoria>()), Times.Never);
    }
}
