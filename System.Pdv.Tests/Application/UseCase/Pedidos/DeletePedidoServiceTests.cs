using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Pdv.Application.UseCase.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.UseCase.Pedidos;

public class DeletePedidoServiceTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly Mock<ILogger<DeletePedidoUseCase>> _loggerMock;
    private readonly DeletePedidoUseCase _deletePedidoService;

    public DeletePedidoServiceTests()
    {
        _pedidoRepositoryMock = new Mock<IPedidoRepository>();
        _loggerMock = new Mock<ILogger<DeletePedidoUseCase>>();

        _deletePedidoService = new DeletePedidoUseCase(
            _pedidoRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task DeletePedido_ShouldReturnSuccess_WhenPedidoExists()
    {
        var pedidoId = Guid.NewGuid();
        var pedido = new Pedido { Id = pedidoId };

        _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(pedidoId))
            .ReturnsAsync(pedido);

        _pedidoRepositoryMock.Setup(r => r.DeleteAsync(pedido))
            .Returns(Task.CompletedTask);

        var result = await _deletePedidoService.ExecuteAsync(pedidoId);

        Assert.NotNull(result.Result);
        Assert.Equal(pedidoId, result.Result.Id);
        Assert.Equal("Pedido deletado com sucesso", result.Message);
        _pedidoRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Pedido>()), Times.Once);
    }

    [Fact]
    public async Task DeletePedido_ShouldReturnNotFound_WhenPedidoDoesNotExist()
    {
        var pedidoId = Guid.NewGuid();

        _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(pedidoId))
            .ReturnsAsync((Pedido)null);

        var result = await _deletePedidoService.ExecuteAsync(pedidoId);

        Assert.Null(result.Result);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Pedido não encontrado", result.Message);
        _pedidoRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Pedido>()), Times.Never);
    }

    [Fact]
    public async Task DeletePedido_ShouldLogError_WhenExceptionOccurs()
    {
        var pedidoId = Guid.NewGuid();

        _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(pedidoId))
            .ThrowsAsync(new Exception("Erro inesperado"));

        var result = await _deletePedidoService.ExecuteAsync(pedidoId);

        Assert.Null(result.Result);
        Assert.Equal(500, result.StatusCode);
        Assert.Equal("Erro inesperado: Erro inesperado", result.Message);
        _pedidoRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Pedido>()), Times.Never);
    }
}
