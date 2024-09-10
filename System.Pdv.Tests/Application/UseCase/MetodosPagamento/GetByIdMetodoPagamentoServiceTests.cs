using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.UseCase.MetodosPagamento;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.UseCase.MetodosPagamento;

public class GetByIdMetodoPagamentoServiceTests
{
    private readonly Mock<IMetodoPagamentoRepository> _metodoPagamentoRepositoryMock;
    private readonly Mock<ILogger<GetByIdMetodoPagamentoUseCase>> _loggerMock;
    private readonly GetByIdMetodoPagamentoUseCase _service;

    public GetByIdMetodoPagamentoServiceTests()
    {
        _metodoPagamentoRepositoryMock = new Mock<IMetodoPagamentoRepository>();
        _loggerMock = new Mock<ILogger<GetByIdMetodoPagamentoUseCase>>();
        _service = new GetByIdMetodoPagamentoUseCase(_metodoPagamentoRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetByIdMetodoPagamento_ShouldReturnNotFound_WhenMetodoPagamentoDoesNotExist()
    {
        var metodoPagamentoId = Guid.NewGuid();
        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetByIdAsync(metodoPagamentoId))
            .ReturnsAsync((MetodoPagamento)null);

        var result = await _service.ExecuteAsync(metodoPagamentoId);

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Método de pagamento não encontrado", result.Message);
        Assert.Null(result.Result);
        _metodoPagamentoRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdMetodoPagamento_ShouldReturnMetodoPagamento_WhenMetodoPagamentoExists()
    {
        var metodoPagamentoId = Guid.NewGuid();
        var metodoPagamento = new MetodoPagamento { Id = metodoPagamentoId, Nome = "Cartão de Débito" };
        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetByIdAsync(metodoPagamentoId)).ReturnsAsync(metodoPagamento);

        var result = await _service.ExecuteAsync(metodoPagamentoId);

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Null(result.Message);
        Assert.Equal(metodoPagamento, result.Result);
        _metodoPagamentoRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdMetodoPagamento_ShouldLogErrorAndReturnServerError_WhenExceptionIsThrown()
    {
        var metodoPagamentoId = Guid.NewGuid();
        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetByIdAsync(metodoPagamentoId)).ThrowsAsync(new Exception("Test exception"));

        var result = await _service.ExecuteAsync(metodoPagamentoId);

        Assert.NotNull(result);
        Assert.False(result.ServerOn);
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("Erro inesperado:", result.Message);
        _metodoPagamentoRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}
