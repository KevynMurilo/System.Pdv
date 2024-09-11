using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Pdv.Core.Entities;
using System.Pdv.Application.UseCase.Pedidos;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.UseCase.Pedidos;

public class PrintPedidoByIdUseCaseTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly Mock<IThermalPrinterService> _thermalPrinterServiceMock;
    private readonly Mock<ILogger<PrintPedidoByIdUseCase>> _loggerMock;
    private readonly PrintPedidoByIdUseCase _printPedidoByIdUseCase;

    public PrintPedidoByIdUseCaseTests()
    {
        _pedidoRepositoryMock = new Mock<IPedidoRepository>();
        _thermalPrinterServiceMock = new Mock<IThermalPrinterService>();
        _loggerMock = new Mock<ILogger<PrintPedidoByIdUseCase>>();
        _printPedidoByIdUseCase = new PrintPedidoByIdUseCase(
            _pedidoRepositoryMock.Object,
            _thermalPrinterServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsPedido_WhenPedidoExistsAndPrintsSuccessfully()
    {
        var pedidoId = Guid.NewGuid();
        var pedido = new Pedido { Id = pedidoId };

        _pedidoRepositoryMock.Setup(repo => repo.GetByIdAsync(pedidoId))
                             .ReturnsAsync(pedido);

        _thermalPrinterServiceMock.Setup(printer => printer.PrintOrder(pedido))
                                  .Returns(true);

        var result = await _printPedidoByIdUseCase.ExecuteAsync(pedidoId);

        Assert.NotNull(result);
        Assert.True(result.ServerOn);
        Assert.Equal(pedido, result.Result);
        Assert.Equal("Pedido impresso com sucesso", result.Message);

        _pedidoRepositoryMock.Verify(repo => repo.GetByIdAsync(pedidoId), Times.Once);
        _thermalPrinterServiceMock.Verify(printer => printer.PrintOrder(pedido), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsError_WhenPedidoNotFound()
    {
        var pedidoId = Guid.NewGuid();

        _pedidoRepositoryMock.Setup(repo => repo.GetByIdAsync(pedidoId))
                             .ReturnsAsync((Pedido)null);

        var result = await _printPedidoByIdUseCase.ExecuteAsync(pedidoId);

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Pedido não encontrado", result.Message);

        _pedidoRepositoryMock.Verify(repo => repo.GetByIdAsync(pedidoId), Times.Once);
        _thermalPrinterServiceMock.Verify(printer => printer.PrintOrder(It.IsAny<Pedido>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsError_WhenPrintFails()
    {
        var pedidoId = Guid.NewGuid();
        var pedido = new Pedido { Id = pedidoId };

        _pedidoRepositoryMock.Setup(repo => repo.GetByIdAsync(pedidoId))
                             .ReturnsAsync(pedido);

        _thermalPrinterServiceMock.Setup(printer => printer.PrintOrder(pedido))
                                  .Returns(false);

        var result = await _printPedidoByIdUseCase.ExecuteAsync(pedidoId);

        Assert.NotNull(result);
        Assert.True(result.ServerOn);
        Assert.Equal(pedido, result.Result);
        Assert.Equal("Ocorreu um erro ao imprimir pedido", result.Message);

        _pedidoRepositoryMock.Verify(repo => repo.GetByIdAsync(pedidoId), Times.Once);
        _thermalPrinterServiceMock.Verify(printer => printer.PrintOrder(pedido), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsServerError_WhenExceptionThrown()
    {
        var pedidoId = Guid.NewGuid();
        var exceptionMessage = "Erro inesperado";

        _pedidoRepositoryMock.Setup(repo => repo.GetByIdAsync(pedidoId))
                             .ThrowsAsync(new Exception(exceptionMessage));

        var result = await _printPedidoByIdUseCase.ExecuteAsync(pedidoId);

        Assert.NotNull(result);
        Assert.False(result.ServerOn);
        Assert.Equal(500, result.StatusCode);
        Assert.Equal($"Erro inesperado: {exceptionMessage}", result.Message);

        _pedidoRepositoryMock.Verify(repo => repo.GetByIdAsync(pedidoId), Times.Once);
        _thermalPrinterServiceMock.Verify(printer => printer.PrintOrder(It.IsAny<Pedido>()), Times.Never);
    }
}
