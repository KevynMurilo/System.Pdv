using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Pdv.Application.UseCase.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.UseCase.Pedidos;

public class GetPedidosByMesaUseCaseTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly Mock<ILogger<GetPedidosByMesaUseCase>> _loggerMock;
    private readonly GetPedidosByMesaUseCase _getPedidosByMesaUseCase;

    public GetPedidosByMesaUseCaseTests()
    {
        _pedidoRepositoryMock = new Mock<IPedidoRepository>();
        _loggerMock = new Mock<ILogger<GetPedidosByMesaUseCase>>();
        _getPedidosByMesaUseCase = new GetPedidosByMesaUseCase(_pedidoRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsPedidos_WhenPedidosExist()
    {
        int numeroMesa = 1;
        string statusPedido = "pendente";

        var mesa = new Mesa
        {
            Id = Guid.NewGuid(),
            Numero = 1
        };

        var pedidos = new List<Pedido>
        {
            new Pedido { Id = Guid.NewGuid(), MesaId = mesa.Id },
            new Pedido { Id = Guid.NewGuid(), MesaId = mesa.Id }
        };

        _pedidoRepositoryMock.Setup(repo => repo.GetPedidosByMesaAsync(numeroMesa, statusPedido))
                             .ReturnsAsync(pedidos);

        var result = await _getPedidosByMesaUseCase.ExecuteAsync(numeroMesa, statusPedido);

        Assert.NotNull(result);
        Assert.True(result.ReqSuccess);
        Assert.Equal(pedidos, result.Result);
        Assert.Null(result.Message);
        _pedidoRepositoryMock.Verify(repo => repo.GetPedidosByMesaAsync(numeroMesa, statusPedido), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsNotFound_WhenNoPedidosExist()
    {
        int numeroMesa = 1;
        string statusPedido = "finalizado";
        var pedidos = new List<Pedido>();

        _pedidoRepositoryMock.Setup(repo => repo.GetPedidosByMesaAsync(numeroMesa, statusPedido))
                             .ReturnsAsync(pedidos);

        var result = await _getPedidosByMesaUseCase.ExecuteAsync(numeroMesa, statusPedido);

        Assert.NotNull(result);
        Assert.True(result.ReqSuccess);
        Assert.Null(result.Result);
        Assert.Equal($"Mesa {numeroMesa} não tem nenhum pedido vinculado", result.Message);
        Assert.Equal(404, result.StatusCode);
        _pedidoRepositoryMock.Verify(repo => repo.GetPedidosByMesaAsync(numeroMesa, statusPedido), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_LogsErrorAndReturnsServerError_WhenExceptionThrown()
    {
        int numeroMesa = 1;
        string statusPedido = "pendente";
        var exception = "Erro ao acessar o banco de dados.";

        _pedidoRepositoryMock.Setup(repo => repo.GetPedidosByMesaAsync(It.IsAny<int>(), It.IsAny<string>()))
                             .ThrowsAsync(new System.Exception(exception));

        var result = await _getPedidosByMesaUseCase.ExecuteAsync(numeroMesa, statusPedido);

        Assert.NotNull(result);
        Assert.False(result.ReqSuccess);
        Assert.Null(result.Result);
        Assert.Equal($"Erro inesperado: {exception}", result.Message);
        Assert.Equal(500, result.StatusCode);
        _pedidoRepositoryMock.Verify(repo => repo.GetPedidosByMesaAsync(numeroMesa, statusPedido), Times.Once);
    }
}
