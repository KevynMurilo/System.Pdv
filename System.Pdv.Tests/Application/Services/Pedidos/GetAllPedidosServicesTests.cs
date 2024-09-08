using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Pdv.Application.Services.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;


namespace System.Pdv.Tests.Application.Services.Pedidos;

public class GetAllPedidosServicesTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly Mock<ILogger<GetAllPedidosServices>> _loggerMock;
    private readonly GetAllPedidosServices _getAllPedidosServices;

    public GetAllPedidosServicesTests()
    {
        _pedidoRepositoryMock = new Mock<IPedidoRepository>();
        _loggerMock = new Mock<ILogger<GetAllPedidosServices>>();

        _getAllPedidosServices = new GetAllPedidosServices(
            _pedidoRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task GetAllPedidos_ShouldReturnPedidos_WhenPedidosExist()
    {
        var pedidos = new List<Pedido>
        {
            new Pedido { Id = Guid.NewGuid() },
            new Pedido { Id = Guid.NewGuid() }
        };

        _pedidoRepositoryMock.Setup(r => r.GetPedidosAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(pedidos);

        var result = await _getAllPedidosServices.ExecuteAsync(1, 10, "tipo", "status");

        Assert.NotNull(result.Result);
        Assert.Equal(pedidos.Count, result.Result.Count());
        Assert.Equal(pedidos[0].Id, result.Result.First().Id);
    }

    [Fact]
    public async Task GetAllPedidos_ShouldReturnNotFound_WhenNoPedidosExist()
    {
        _pedidoRepositoryMock.Setup(r => r.GetPedidosAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new List<Pedido>());

        var result = await _getAllPedidosServices.ExecuteAsync(1, 10, "tipo", "status");

        Assert.Null(result.Result);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Nenhum pedido encontrado", result.Message);
    }

    [Fact]
    public async Task GetAllPedidos_ShouldLogError_WhenExceptionOccurs()
    {
        _pedidoRepositoryMock.Setup(r => r.GetPedidosAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("Erro inesperado"));

        var result = await _getAllPedidosServices.ExecuteAsync(1, 10, "tipo", "status");

        Assert.Null(result.Result);
        Assert.Equal(500, result.StatusCode);
        Assert.Equal("Erro inesperado: Erro inesperado", result.Message);
    }
}
