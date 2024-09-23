using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.UseCase.MetodosPagamento;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.UseCase.MetodosPagamento;

public class DeleteMetodoPagamentoServiceTests
{
    private readonly Mock<IMetodoPagamentoRepository> _metodoPagamentoRepositoryMock;
    private readonly Mock<ILogger<DeleteMetodoPagamentoUseCase>> _loggerMock;
    private readonly DeleteMetodoPagamentoUseCase _service;

    public DeleteMetodoPagamentoServiceTests()
    {
        _metodoPagamentoRepositoryMock = new Mock<IMetodoPagamentoRepository>();
        _loggerMock = new Mock<ILogger<DeleteMetodoPagamentoUseCase>>();
        _service = new DeleteMetodoPagamentoUseCase(_metodoPagamentoRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task DeleteMetodoPagamento_ShouldReturnNotFound_WhenMetodoPagamentoDoesNotExist()
    {
        var id = Guid.NewGuid();
        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((MetodoPagamento)null);

        var result = await _service.ExecuteAsync(id);

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Método de pagamento não encontrado", result.Message);
        Assert.Null(result.Result);
        _metodoPagamentoRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task DeleteMetodoPagamento_ShouldReturnSuccess_WhenMetodoPagamentoIsDeletedSuccessfully()
    {
        var id = Guid.NewGuid();
        var metodoPagamento = new MetodoPagamento { Id = id, Nome = "Pix" };
        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(metodoPagamento);
        _metodoPagamentoRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<MetodoPagamento>())).Returns(Task.CompletedTask);

        var result = await _service.ExecuteAsync(id);

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("Método de pagamento deletado com sucesso", result.Message);
        Assert.Equal(metodoPagamento, result.Result);
        _metodoPagamentoRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _metodoPagamentoRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<MetodoPagamento>()), Times.Once);
    }

    [Fact]
    public async Task DeleteMetodoPagamento_ShouldLogErrorAndReturnServerError_WhenExceptionIsThrown()
    {
        var id = Guid.NewGuid();
        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("Test exception"));

        var result = await _service.ExecuteAsync(id);

        Assert.NotNull(result);
        Assert.False(result.ReqSuccess);
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("Erro inesperado:", result.Message);
        _metodoPagamentoRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}
