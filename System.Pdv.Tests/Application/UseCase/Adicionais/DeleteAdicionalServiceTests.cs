using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Pdv.Application.UseCase.Adicionais;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.UseCase.Adicionais;
public class DeleteAdicionalServiceTests
{
    private readonly Mock<IAdicionalRepository> _adicionalRepositoryMock;
    private readonly Mock<ILogger<DeleteAdicionalUseCase>> _loggerMock;
    private readonly DeleteAdicionalUseCase _deleteAdicionalService;

    public DeleteAdicionalServiceTests()
    {
        _adicionalRepositoryMock = new Mock<IAdicionalRepository>();
        _loggerMock = new Mock<ILogger<DeleteAdicionalUseCase>>();
        _deleteAdicionalService = new DeleteAdicionalUseCase(_adicionalRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task DeleteAdicional_ShouldReturnNotFound_WhenAdicionalDoesNotExist()
    {
        var adicionalId = Guid.NewGuid();
        _adicionalRepositoryMock.Setup(repo => repo.GetByIdAsync(adicionalId))
            .ReturnsAsync((ItemAdicional)null);

        var result = await _deleteAdicionalService.ExecuteAsync(adicionalId);

        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Adicional não encontrado", result.Message);
        _adicionalRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _adicionalRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<ItemAdicional>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAdicional_ShouldReturnSuccess_WhenAdicionalIsDeletedSuccessfully()
    {
        var adicionalId = Guid.NewGuid();
        var adicional = new ItemAdicional { Id = adicionalId, Nome = "Adicional Teste" };

        _adicionalRepositoryMock.Setup(repo => repo.GetByIdAsync(adicionalId))
            .ReturnsAsync(adicional);

        _adicionalRepositoryMock.Setup(repo => repo.DeleteAsync(adicional))
            .Returns(Task.CompletedTask);

        var result = await _deleteAdicionalService.ExecuteAsync(adicionalId);

        Assert.NotNull(result.Result);
        Assert.Equal(adicionalId, result.Result.Id);
        Assert.Equal("Adicional deletado com sucesso", result.Message);
        Assert.Equal(200, result.StatusCode);
        _adicionalRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _adicionalRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<ItemAdicional>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAdicional_ShouldLogErrorAndReturnError_WhenExceptionIsThrown()
    {
        var adicionalId = Guid.NewGuid();
        var exception = new Exception("Database error");

        _adicionalRepositoryMock.Setup(repo => repo.GetByIdAsync(adicionalId))
            .ThrowsAsync(exception);

        var result = await _deleteAdicionalService.ExecuteAsync(adicionalId);

        Assert.False(result.ServerOn);
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("Erro inesperado", result.Message);
        _adicionalRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _adicionalRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<ItemAdicional>()), Times.Never);
    }
}
