using Moq;
using Microsoft.Extensions.Logging;
using System.Pdv.Application.UseCase.StatusPedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.UseCase.StatusPedidos;

public class GetByIdStatusPedidoServiceTests
{
    private readonly Mock<IStatusPedidoRepository> _statusPedidoRepositoryMock;
    private readonly Mock<ILogger<GetByIdStatusPedidoUseCase>> _loggerMock;
    private readonly GetByIdStatusPedidoUseCase _service;

    public GetByIdStatusPedidoServiceTests()
    {
        _statusPedidoRepositoryMock = new Mock<IStatusPedidoRepository>();
        _loggerMock = new Mock<ILogger<GetByIdStatusPedidoUseCase>>();
        _service = new GetByIdStatusPedidoUseCase(_statusPedidoRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenStatusPedidoDoesNotExist()
    {
        var id = Guid.NewGuid();
        _statusPedidoRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync((StatusPedido)null);

        var result = await _service.ExecuteAsync(id);

        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Status de pedido não encontrado", result.Message);
        _statusPedidoRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnStatusPedido_WhenStatusPedidoExists()
    {
        var id = Guid.NewGuid();
        var statusPedido = new StatusPedido { Id = id, Status = "Pending" };
        _statusPedidoRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(statusPedido);

        var result = await _service.ExecuteAsync(id);

        Assert.Null(result.Message);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(statusPedido, result.Result);
        _statusPedidoRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnServerError_WhenExceptionIsThrown()
    {
        var id = Guid.NewGuid();
        _statusPedidoRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
            .ThrowsAsync(new Exception("Database error"));

        var result = await _service.ExecuteAsync(id);

        Assert.False(result.ReqSuccess);
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("Erro inesperado", result.Message);
        _statusPedidoRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
    }
}
