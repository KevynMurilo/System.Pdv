using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.UseCase.Categorias;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.UseCase.Categorias;

public class CreateCategoriaServiceTests
{
    private readonly Mock<ICategoriaRepository> _repositoryMock;
    private readonly Mock<ILogger<CreateCategoriaUseCase>> _loggerMock;
    private readonly CreateCategoriaUseCase _service;

    public CreateCategoriaServiceTests()
    {
        _repositoryMock = new Mock<ICategoriaRepository>();
        _loggerMock = new Mock<ILogger<CreateCategoriaUseCase>>();
        _service = new CreateCategoriaUseCase(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task CreateCategoria_ShouldReturnConflict_WhenCategoriaExists()
    {
        var categoriaDto = new CategoriaDto { Nome = "Lanches" };
        var existingCategoria = new Categoria { Nome = categoriaDto.Nome };

        _repositoryMock.Setup(repo => repo.GetByNameAsync(categoriaDto.Nome))
            .ReturnsAsync(existingCategoria);

        var result = await _service.ExecuteAsync(categoriaDto);

        Assert.NotNull(result);
        Assert.True(result.ReqSuccess);
        Assert.Equal(409, result.StatusCode);
        Assert.Equal("Categoria já registrada", result.Message);
        _repositoryMock.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
        _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Categoria>()), Times.Never);
    }

    [Fact]
    public async Task CreateCategoria_ShouldCreateCategoria_WhenCategoriaDoesNotExist()
    {
        var categoriaDto = new CategoriaDto { Nome = "Lanches" };

        _repositoryMock.Setup(repo => repo.GetByNameAsync(categoriaDto.Nome))
            .ReturnsAsync((Categoria)null);

        var result = await _service.ExecuteAsync(categoriaDto);

        Assert.NotNull(result);
        Assert.True(result.ReqSuccess);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(categoriaDto.Nome.ToUpper(), result.Result.Nome);
        _repositoryMock.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
        _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Categoria>()), Times.Once);
    }

    [Fact]
    public async Task CreateCategoria_ShouldReturnError_WhenExceptionIsThrown()
    {
        var categoriaDto = new CategoriaDto { Nome = "Lanches" };

        _repositoryMock.Setup(repo => repo.GetByNameAsync(categoriaDto.Nome))
            .ThrowsAsync(new Exception("Erro de banco de dados"));

        var result = await _service.ExecuteAsync(categoriaDto);

        Assert.False(result.ReqSuccess);
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("Erro inesperado", result.Message);
        _repositoryMock.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
        _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Categoria>()), Times.Never);
    }
}
