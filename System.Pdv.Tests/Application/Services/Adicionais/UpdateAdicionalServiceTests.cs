using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Pdv.Application.Services.Adicionais;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Services.Adicionais;

public class UpdateAdicionalServiceTests
{
    private readonly Mock<IAdicionalRepository> _adicionalRepositoryMock;
    private readonly Mock<ILogger<UpdateAdicionalService>> _loggerMock;
    private readonly UpdateAdicionalService _updateAdicionalService;

    public UpdateAdicionalServiceTests()
    {
        _adicionalRepositoryMock = new Mock<IAdicionalRepository>();
        _loggerMock = new Mock<ILogger<UpdateAdicionalService>>();
        _updateAdicionalService = new UpdateAdicionalService(_adicionalRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task UpdateAdicional_ShouldReturnNotFound_WhenAdicionalDoesNotExist()
    {
        var adicionalId = Guid.NewGuid();
        var adicionalDto = new AdicionalDto { Nome = "Updated Name", Preco = 15.0m };

        _adicionalRepositoryMock.Setup(repo => repo.GetByIdAsync(adicionalId))
            .ReturnsAsync((ItemAdicional)null);

        var result = await _updateAdicionalService.ExecuteAsync(adicionalId, adicionalDto);

        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Adicional não encontrado", result.Message);
        _adicionalRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAdicional_ShouldReturnSuccess_WhenAdicionalIsUpdatedSuccessfully()
    {
        var adicionalId = Guid.NewGuid();
        var adicional = new ItemAdicional { Id = adicionalId, Nome = "OLD NAME", Preco = 10.0m };
        var adicionalDto = new AdicionalDto { Nome = "Updated Name", Preco = 15.0m };

        _adicionalRepositoryMock.Setup(repo => repo.GetByIdAsync(adicionalId))
            .ReturnsAsync(adicional);

        _adicionalRepositoryMock.Setup(repo => repo.UpdateAsync(adicional))
            .Returns(Task.CompletedTask);

        var result = await _updateAdicionalService.ExecuteAsync(adicionalId, adicionalDto);

        Assert.Equal(adicionalId, result.Result.Id);
        Assert.Equal(adicional.Nome, result.Result.Nome);
        Assert.Equal(15.0m, result.Result.Preco);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("Adicional atualizado com sucesso", result.Message);
        _adicionalRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _adicionalRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<ItemAdicional>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAdicional_ShouldLogErrorAndReturnError_WhenExceptionIsThrown()
    {
        var adicionalId = Guid.NewGuid();
        var adicionalDto = new AdicionalDto { Nome = "Updated Name", Preco = 15.0m };
        var exception = new Exception("Database error");

        _adicionalRepositoryMock.Setup(repo => repo.GetByIdAsync(adicionalId))
            .ThrowsAsync(exception);

        var result = await _updateAdicionalService.ExecuteAsync(adicionalId, adicionalDto);

        Assert.False(result.ServerOn);
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("Erro inesperado", result.Message);
        _adicionalRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}
