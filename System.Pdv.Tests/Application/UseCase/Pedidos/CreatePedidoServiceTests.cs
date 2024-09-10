using Moq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.UseCase.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
using System.Pdv.Core.Interfaces;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Pedidos;

namespace System.Pdv.Tests.Application.UseCase.Pedidos;

public class CreatePedidoUseCaseTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly Mock<IProcessarItensPedidoUseCase> _processarItensPedidoMock;
    private readonly Mock<IValidarPedidosUseCase> _validarPedidosServiceMock;
    private readonly Mock<ITransactionManager> _transactionManagerMock;
    private readonly Mock<IThermalPrinterService> _thermalPrinterServiceMock;
    private readonly Mock<ILogger<CreatePedidoUseCase>> _loggerMock;
    private readonly CreatePedidoUseCase _createPedidoUseCase;

    public CreatePedidoUseCaseTests()
    {
        _pedidoRepositoryMock = new Mock<IPedidoRepository>();
        _processarItensPedidoMock = new Mock<IProcessarItensPedidoUseCase>();
        _validarPedidosServiceMock = new Mock<IValidarPedidosUseCase>();
        _transactionManagerMock = new Mock<ITransactionManager>();
        _thermalPrinterServiceMock = new Mock<IThermalPrinterService>();
        _loggerMock = new Mock<ILogger<CreatePedidoUseCase>>();

        _createPedidoUseCase = new CreatePedidoUseCase(
            _pedidoRepositoryMock.Object,
            _processarItensPedidoMock.Object,
            _validarPedidosServiceMock.Object,
            _transactionManagerMock.Object,
            _thermalPrinterServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task CreatePedido_ShouldReturnPedido_WhenPedidoIsValid()
    {
        var pedidoDto = new PedidoDto
        {
            NomeCliente = "Cliente Teste",
            TelefoneCliente = "11999999999",
            Itens = new List<ItemPedidoDto>(),
            TipoPedido = TipoPedido.Interno,
            MesaId = Guid.NewGuid(),
            MetodoPagamentoId = Guid.NewGuid(),
            StatusPedidoId = Guid.NewGuid()
        };

        var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("id", Guid.NewGuid().ToString()) }));
        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            Cliente = new Cliente { Nome = pedidoDto.NomeCliente, Telefone = pedidoDto.TelefoneCliente },
            TipoPedido = pedidoDto.TipoPedido,
            MesaId = pedidoDto.MesaId,
            MetodoPagamentoId = pedidoDto.MetodoPagamentoId
        };

        _validarPedidosServiceMock.Setup(v => v.ValidarPedido(It.IsAny<PedidoDto>(), It.IsAny<string>()))
            .ReturnsAsync((OperationResult<Pedido>)null);

        _processarItensPedidoMock.Setup(p => p.ExecuteAsync(It.IsAny<List<ItemPedidoDto>>(), It.IsAny<Pedido>()))
            .ReturnsAsync((OperationResult<Pedido>)null);

        _pedidoRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Pedido>()))
            .Returns(Task.CompletedTask);

        _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(pedido);

        _thermalPrinterServiceMock.Setup(t => t.PrintOrder(It.IsAny<Pedido>()))
            .Returns(true);

        var result = await _createPedidoUseCase.ExecuteAsync(pedidoDto, userClaims);

        Assert.NotNull(result.Result);
        Assert.Equal(pedidoDto.NomeCliente, result.Result.Cliente.Nome);
        Assert.Equal(pedidoDto.TelefoneCliente, result.Result.Cliente.Telefone);
        Assert.Equal(pedidoDto.TipoPedido, result.Result.TipoPedido);
        Assert.Equal(pedidoDto.MesaId, result.Result.MesaId);
        Assert.Equal(pedidoDto.MetodoPagamentoId, result.Result.MetodoPagamentoId);
        _pedidoRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Pedido>()), Times.Once);
        _transactionManagerMock.Verify(t => t.CommitTransactionAsync(), Times.Once);
        _thermalPrinterServiceMock.Verify(t => t.PrintOrder(It.IsAny<Pedido>()), Times.Once);
    }

    [Fact]
    public async Task CreatePedido_ShouldReturnError_WhenValidationFails()
    {
        var pedidoDto = new PedidoDto
        {
            NomeCliente = "Cliente Teste",
            TelefoneCliente = "11999999999",
            Itens = new List<ItemPedidoDto>(),
            TipoPedido = TipoPedido.Interno
        };

        var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("id", Guid.NewGuid().ToString()) }));
        var validationResult = new OperationResult<Pedido> { Message = "Erro de validação", StatusCode = 400 };

        _validarPedidosServiceMock.Setup(v => v.ValidarPedido(It.IsAny<PedidoDto>(), It.IsAny<string>()))
            .ReturnsAsync(validationResult);

        var result = await _createPedidoUseCase.ExecuteAsync(pedidoDto, userClaims);

        Assert.Null(result.Result);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Erro de validação", result.Message);

        _pedidoRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Pedido>()), Times.Never);
        _transactionManagerMock.Verify(t => t.CommitTransactionAsync(), Times.Never);
        _thermalPrinterServiceMock.Verify(t => t.PrintOrder(It.IsAny<Pedido>()), Times.Never);
    }

    [Fact]
    public async Task CreatePedido_ShouldLogError_WhenExceptionOccurs()
    {
        var pedidoDto = new PedidoDto
        {
            NomeCliente = "Cliente Teste",
            TelefoneCliente = "11999999999",
            Itens = new List<ItemPedidoDto>(),
            TipoPedido = TipoPedido.Interno
        };

        var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("id", Guid.NewGuid().ToString()) }));

        _validarPedidosServiceMock.Setup(v => v.ValidarPedido(It.IsAny<PedidoDto>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("Erro inesperado"));

        var result = await _createPedidoUseCase.ExecuteAsync(pedidoDto, userClaims);

        Assert.Null(result.Result);
        Assert.Equal(500, result.StatusCode);
        Assert.Equal("Erro inesperado: Erro inesperado", result.Message);
        _pedidoRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Pedido>()), Times.Never);
        _transactionManagerMock.Verify(t => t.RollbackTransactionAsync(), Times.Once);
        _thermalPrinterServiceMock.Verify(t => t.PrintOrder(It.IsAny<Pedido>()), Times.Never);
    }
}
