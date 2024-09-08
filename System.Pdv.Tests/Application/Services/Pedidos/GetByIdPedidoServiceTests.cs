using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Pdv.Application.Services.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.Services.Pedidos;

public class GetByIdPedidoServiceTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly Mock<ILogger<GetByIdPedidoService>> _loggerMock;
    private readonly GetByIdPedidoService _getByIdPedidoService;

    public GetByIdPedidoServiceTests()
    {
        _pedidoRepositoryMock = new Mock<IPedidoRepository>();
        _loggerMock = new Mock<ILogger<GetByIdPedidoService>>();

        _getByIdPedidoService = new GetByIdPedidoService(
            _pedidoRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task GetByIdPedido_ShouldReturnPedido_WhenPedidoExists()
    {
        var pedidoId = Guid.NewGuid();
        var pedido = new Pedido { Id = pedidoId };

        _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(pedidoId))
            .ReturnsAsync(pedido);

        var result = await _getByIdPedidoService.ExecuteAsync(pedidoId);

        Assert.NotNull(result.Result);
        Assert.Equal(pedidoId, result.Result.Id);
        Assert.Null(result.Message);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task GetByIdPedido_ShouldReturnNotFound_WhenPedidoDoesNotExist()
    {
        var pedidoId = Guid.NewGuid();

        _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(pedidoId))
            .ReturnsAsync((Pedido)null);

        var result = await _getByIdPedidoService.ExecuteAsync(pedidoId);

        Assert.Null(result.Result);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Pedido não encontrado", result.Message);
    }

    [Fact]
    public async Task GetByIdPedido_ShouldLogError_WhenExceptionOccurs()
    {
        var pedidoId = Guid.NewGuid();

        _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(pedidoId))
            .ThrowsAsync(new Exception("Erro inesperado"));

        var result = await _getByIdPedidoService.ExecuteAsync(pedidoId);

        Assert.Null(result.Result);
        Assert.Equal(500, result.StatusCode);
        Assert.Equal("Erro inesperado: Erro inesperado", result.Message);
    }
}
