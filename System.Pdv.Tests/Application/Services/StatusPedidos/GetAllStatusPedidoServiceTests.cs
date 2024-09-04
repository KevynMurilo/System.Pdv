using Moq;
using Microsoft.Extensions.Logging;
using System.Pdv.Application.Services.StatusPedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Services.StatusPedidos;

public class GetAllStatusPedidoServiceTests
{
    private readonly Mock<IStatusPedidoRepository> _statusPedidoRepositoryMock;
    private readonly Mock<ILogger<GetAllStatusPedidoService>> _loggerMock;
    private readonly GetAllStatusPedidoService _service;

    public GetAllStatusPedidoServiceTests()
    {
        _statusPedidoRepositoryMock = new Mock<IStatusPedidoRepository>();
        _loggerMock = new Mock<ILogger<GetAllStatusPedidoService>>();
        _service = new GetAllStatusPedidoService(_statusPedidoRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenNoStatusPedidoExists()
    {
        _statusPedidoRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<StatusPedido>());

        var result = await _service.ExecuteAsync();

        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Nenhum status de pedido encontrado", result.Message);
        _statusPedidoRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnStatusPedidos_WhenStatusPedidosExist()
    {
        var statusPedidos = new List<StatusPedido>
        {
            new StatusPedido { Status = "Pending" },
            new StatusPedido { Status = "Completed" }
        };
        _statusPedidoRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(statusPedidos);

        var result = await _service.ExecuteAsync();

        Assert.Null(result.Message);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(statusPedidos, result.Result);
        _statusPedidoRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnServerError_WhenExceptionIsThrown()
    {
        _statusPedidoRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ThrowsAsync(new Exception("Database error"));

        var result = await _service.ExecuteAsync();

        Assert.False(result.ServerOn);
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("Erro inesperado", result.Message);
        _statusPedidoRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }
}
