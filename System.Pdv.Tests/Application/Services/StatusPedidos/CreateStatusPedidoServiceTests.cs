using Moq;
using Microsoft.Extensions.Logging;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Services.StatusPedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Services.StatusPedidos;

public class CreateStatusPedidoServiceTests
{
    private readonly Mock<IStatusPedidoRepository> _statusPedidoRepositoryMock;
    private readonly Mock<ILogger<CreateStatusPedidoService>> _loggerMock;
    private readonly CreateStatusPedidoService _service;

    public CreateStatusPedidoServiceTests()
    {
        _statusPedidoRepositoryMock = new Mock<IStatusPedidoRepository>();
        _loggerMock = new Mock<ILogger<CreateStatusPedidoService>>();
        _service = new CreateStatusPedidoService(_statusPedidoRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnConflict_WhenStatusAlreadyExists()
    {
        var statusPedidoDto = new StatusPedidoDto { Status = "Pending" };
        _statusPedidoRepositoryMock.Setup(repo => repo.GetByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new StatusPedido());

        var result = await _service.ExecuteAsync(statusPedidoDto);

        Assert.Equal(409, result.StatusCode);
        Assert.Equal("Status de pedido já registrado", result.Message);
        _statusPedidoRepositoryMock.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
        _statusPedidoRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<StatusPedido>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCreateStatus_WhenStatusDoesNotExist()
    {
        var statusPedidoDto = new StatusPedidoDto { Status = "Pending" };
        _statusPedidoRepositoryMock.Setup(repo => repo.GetByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((StatusPedido)null);

        _statusPedidoRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<StatusPedido>()))
            .Returns(Task.CompletedTask);

        var result = await _service.ExecuteAsync(statusPedidoDto);

        Assert.Null(result.Message);
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Result);
        Assert.Equal("PENDING", result.Result.Status);
        _statusPedidoRepositoryMock.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
        _statusPedidoRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<StatusPedido>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnServerError_WhenExceptionIsThrown()
    {
        var statusPedidoDto = new StatusPedidoDto { Status = "Pending" };
        _statusPedidoRepositoryMock.Setup(repo => repo.GetByNameAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Database error"));

        var result = await _service.ExecuteAsync(statusPedidoDto);

        Assert.False(result.ServerOn);
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("Erro inesperado", result.Message);
        _statusPedidoRepositoryMock.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
    }
}
