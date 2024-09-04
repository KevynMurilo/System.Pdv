using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Services.Categorias;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.Services.Categorias;
public class UpdateCategoriaServiceTests
{
    private readonly Mock<ICategoriaRepository> _categoriasRepositoryMock;
    private readonly Mock<ILogger<UpdateCategoriaService>> _loggerMock;
    private readonly UpdateCategoriaService _service;

    public UpdateCategoriaServiceTests()
    {
        _categoriasRepositoryMock = new Mock<ICategoriaRepository>();
        _loggerMock = new Mock<ILogger<UpdateCategoriaService>>();
        _service = new UpdateCategoriaService(_categoriasRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task UpdateCategoria_CategoriaExists_ReturnsUpdatedCategoria()
    {
        var id = Guid.NewGuid();
        var categoriaDto = new CategoriaDto { Nome = "Updated Name" };
        var categoria = new Categoria { Id = id, Nome = "Original Name" };

        _categoriasRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(categoria);
        _categoriasRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Categoria>())).Returns(Task.CompletedTask);

        var result = await _service.ExecuteAsync(id, categoriaDto);

        Assert.NotNull(result);
        Assert.True(result.ServerOn);
        Assert.Equal(categoriaDto.Nome.ToUpper(), result.Result.Nome);
        _categoriasRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        _categoriasRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Categoria>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCategoria_CategoriaDoesNotExist_ReturnsNotFound()
    {
        var id = Guid.NewGuid();
        var categoriaDto = new CategoriaDto { Nome = "Updated Name" };

        _categoriasRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Categoria)null);

        var result = await _service.ExecuteAsync(id, categoriaDto);

        Assert.NotNull(result);
        Assert.True(result.ServerOn);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Categoria não encontrada", result.Message);
        _categoriasRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task UpdateCategoria_WhenExceptionThrown_LogsErrorAndReturnsServerError()
    {
        var id = Guid.NewGuid();
        var categoriaDto = new CategoriaDto { Nome = "Updated Name" };

        _categoriasRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
            .ThrowsAsync(new Exception("Test exception"));

        var result = await _service.ExecuteAsync(id, categoriaDto);

        Assert.NotNull(result);
        Assert.False(result.ServerOn);
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("Erro inesperado:", result.Message);
        _categoriasRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
    }
}
