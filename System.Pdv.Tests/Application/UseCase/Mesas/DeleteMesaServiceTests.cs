using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Pdv.Application.UseCase.Mesas;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.UseCase.Mesas;
public class DeleteMesaServiceTests
{
    private readonly Mock<IMesaRepository> _mockMesaRepository;
    private readonly Mock<ILogger<DeleteMesaUseCase>> _mockLogger;
    private readonly DeleteMesaUseCase _deleteMesaService;

    public DeleteMesaServiceTests()
    {
        _mockMesaRepository = new Mock<IMesaRepository>();
        _mockLogger = new Mock<ILogger<DeleteMesaUseCase>>();
        _deleteMesaService = new DeleteMesaUseCase(_mockMesaRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task DeleteMesa_ShouldReturnNotFound_WhenMesaDoesNotExist()
    {
        var id = Guid.NewGuid();
        _mockMesaRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Mesa)null);

        var result = await _deleteMesaService.ExecuteAsync(id);

        Assert.True(result.ReqSuccess);
        Assert.Equal("Mesa não encontrada", result.Message);
        Assert.Equal(404, result.StatusCode);
        _mockMesaRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task DeleteMesa_ShouldDeleteMesa_WhenMesaExists()
    {
        var id = Guid.NewGuid();
        var mesa = new Mesa { Id = id };
        _mockMesaRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(mesa);

        var result = await _deleteMesaService.ExecuteAsync(id);

        _mockMesaRepository.Verify(repo => repo.DeleteAsync(mesa), Times.Once);
        Assert.True(result.ReqSuccess);
        Assert.Equal("Mesa deletada com sucesso!", result.Message);
        _mockMesaRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mockMesaRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Mesa>()), Times.Once);
    }

    [Fact]
    public async Task DeleteMesa_ShouldLogError_WhenExceptionIsThrown()
    {
        var id = Guid.NewGuid();
        var exception = new Exception("Database error");
        _mockMesaRepository.Setup(repo => repo.GetByIdAsync(id)).ThrowsAsync(exception);

        var result = await _deleteMesaService.ExecuteAsync(id);

        Assert.False(result.ReqSuccess);
        Assert.Equal("Erro inesperado: Database error", result.Message);
        Assert.Equal(500, result.StatusCode);
        _mockMesaRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}
