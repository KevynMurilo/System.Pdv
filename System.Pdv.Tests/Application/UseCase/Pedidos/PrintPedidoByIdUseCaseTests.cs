using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Pdv.Core.Entities;
using System.Pdv.Application.UseCase.Pedidos;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.UseCase.Pedidos;

public class PrintPedidoByIdsUseCaseTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly Mock<IThermalPrinterService> _thermalPrinterServiceMock;
    private readonly Mock<ILogger<PrintPedidoByIdsUseCase>> _loggerMock;
    private readonly PrintPedidoByIdsUseCase _printPedidoByIdsUseCase;

    public PrintPedidoByIdsUseCaseTests()
    {
        _pedidoRepositoryMock = new Mock<IPedidoRepository>();
        _thermalPrinterServiceMock = new Mock<IThermalPrinterService>();
        _loggerMock = new Mock<ILogger<PrintPedidoByIdsUseCase>>();
        _printPedidoByIdsUseCase = new PrintPedidoByIdsUseCase(
            _pedidoRepositoryMock.Object,
            _thermalPrinterServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsPedidos_WhenPedidosExistAndPrintsSuccessfully()
    {
        var pedidoIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var pedidos = pedidoIds.Select(id => new Pedido { Id = id }).ToList();

        _pedidoRepositoryMock.Setup(repo => repo.GetPedidosByIdsAsync(pedidoIds))
                             .ReturnsAsync(pedidos);

        _thermalPrinterServiceMock.Setup(printer => printer.PrintOrders(pedidos))
                                  .Returns(true);

        var result = await _printPedidoByIdsUseCase.ExecuteAsync(pedidoIds);

        Assert.NotNull(result);
        Assert.True(result.ServerOn);
        Assert.Equal(pedidos, result.Result);
        Assert.Equal("Pedidos impressos com sucesso", result.Message);

        _pedidoRepositoryMock.Verify(repo => repo.GetPedidosByIdsAsync(pedidoIds), Times.Once);
        _thermalPrinterServiceMock.Verify(printer => printer.PrintOrders(pedidos), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsError_WhenPedidosNotFound()
    {
        var pedidoIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        _pedidoRepositoryMock.Setup(repo => repo.GetPedidosByIdsAsync(pedidoIds))
                             .ReturnsAsync(new List<Pedido>());

        var result = await _printPedidoByIdsUseCase.ExecuteAsync(pedidoIds);

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Nenhum pedido encontrado", result.Message);

        _pedidoRepositoryMock.Verify(repo => repo.GetPedidosByIdsAsync(pedidoIds), Times.Once);
        _thermalPrinterServiceMock.Verify(printer => printer.PrintOrders(It.IsAny<List<Pedido>>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsError_WhenPrintFails()
    {
        var pedidoIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var pedidos = pedidoIds.Select(id => new Pedido { Id = id }).ToList();

        _pedidoRepositoryMock.Setup(repo => repo.GetPedidosByIdsAsync(pedidoIds))
                             .ReturnsAsync(pedidos);

        _thermalPrinterServiceMock.Setup(printer => printer.PrintOrders(pedidos))
                                  .Returns(false);

        var result = await _printPedidoByIdsUseCase.ExecuteAsync(pedidoIds);

        Assert.NotNull(result);
        Assert.True(result.ServerOn);
        Assert.Equal(pedidos, result.Result);
        Assert.Equal("Ocorreu um erro ao imprimir os pedidos", result.Message);

        _pedidoRepositoryMock.Verify(repo => repo.GetPedidosByIdsAsync(pedidoIds), Times.Once);
        _thermalPrinterServiceMock.Verify(printer => printer.PrintOrders(pedidos), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsServerError_WhenExceptionThrown()
    {
        var pedidoIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var exceptionMessage = "Erro inesperado";

        _pedidoRepositoryMock.Setup(repo => repo.GetPedidosByIdsAsync(pedidoIds))
                             .ThrowsAsync(new Exception(exceptionMessage));

        var result = await _printPedidoByIdsUseCase.ExecuteAsync(pedidoIds);

        Assert.NotNull(result);
        Assert.False(result.ServerOn);
        Assert.Equal(500, result.StatusCode);
        Assert.Equal($"Erro inesperado: {exceptionMessage}", result.Message);

        _pedidoRepositoryMock.Verify(repo => repo.GetPedidosByIdsAsync(pedidoIds), Times.Once);
        _thermalPrinterServiceMock.Verify(printer => printer.PrintOrders(It.IsAny<List<Pedido>>()), Times.Never);
    }
}
