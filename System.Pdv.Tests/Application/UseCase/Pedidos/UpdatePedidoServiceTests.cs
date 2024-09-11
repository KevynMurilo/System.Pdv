using Moq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Pdv.Application.UseCase.Pedidos;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
using System.Pdv.Core.Interfaces;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Pedidos;

namespace System.Pdv.Tests.Application.UseCase.Pedidos;

public class UpdatePedidoServiceTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly Mock<IProcessarItensPedidoUseCase> _processarItensPedidoMock;
    private readonly Mock<IValidarPedidosUseCase> _validarPedidosServiceMock;
    private readonly Mock<IThermalPrinterService> _thermalPrinterServiceMock;
    private readonly Mock<ITransactionManager> _transactionManagerMock;
    private readonly Mock<ILogger<UpdatePedidoUseCase>> _loggerMock;
    private readonly UpdatePedidoUseCase _updatePedidoService;

    public UpdatePedidoServiceTests()
    {
        _pedidoRepositoryMock = new Mock<IPedidoRepository>();
        _processarItensPedidoMock = new Mock<IProcessarItensPedidoUseCase>();
        _validarPedidosServiceMock = new Mock<IValidarPedidosUseCase>();
        _thermalPrinterServiceMock = new Mock<IThermalPrinterService>();
        _transactionManagerMock = new Mock<ITransactionManager>();
        _loggerMock = new Mock<ILogger<UpdatePedidoUseCase>>();

        _updatePedidoService = new UpdatePedidoUseCase(
            _pedidoRepositoryMock.Object,
            _processarItensPedidoMock.Object,
            _validarPedidosServiceMock.Object,
            _thermalPrinterServiceMock.Object,
            _transactionManagerMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task UpdatePedido_ShouldReturnUpdatedPedido_WhenPedidoIsValid()
    {
        var pedidoId = Guid.NewGuid();
        var pedidoDto = new PedidoDto
        {
            NomeCliente = "Cliente Atualizado",
            TelefoneCliente = "11999999998",
            Itens = new List<ItemPedidoDto>(),
            TipoPedido = TipoPedido.Interno,
            MesaId = Guid.NewGuid(),
            MetodoPagamentoId = Guid.NewGuid(),
            StatusPedidoId = Guid.NewGuid()
        };

        var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("id", Guid.NewGuid().ToString()) }));
        var pedido = new Pedido
        {
            Id = pedidoId,
            Cliente = new Cliente { Nome = "Cliente Antigo", Telefone = "11999999999" },
            Items = new List<ItemPedido>()
        };

        _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(pedidoId))
            .ReturnsAsync(pedido);

        _validarPedidosServiceMock.Setup(v => v.ValidarPedido(It.IsAny<PedidoDto>(), It.IsAny<string>()))
            .ReturnsAsync((OperationResult<Pedido>)null);

        _processarItensPedidoMock.Setup(p => p.ExecuteAsync(It.IsAny<List<ItemPedidoDto>>(), It.IsAny<Pedido>()))
            .ReturnsAsync((OperationResult<Pedido>)null);

        _pedidoRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Pedido>()))
            .Returns(Task.CompletedTask);

        _thermalPrinterServiceMock.Setup(t => t.PrintOrder(It.IsAny<Pedido>()))
            .Returns(true);

        var result = await _updatePedidoService.ExecuteAsync(pedidoId, pedidoDto, userClaims);

        Assert.NotNull(result.Result);
        Assert.Equal(pedidoDto.NomeCliente, result.Result.Cliente.Nome);
        Assert.Equal(pedidoDto.TelefoneCliente, result.Result.Cliente.Telefone);
        _pedidoRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Pedido>()), Times.Once);
        _transactionManagerMock.Verify(t => t.CommitTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdatePedido_ShouldReturnNotFound_WhenPedidoDoesNotExist()
    {
        var pedidoId = Guid.NewGuid();
        var pedidoDto = new PedidoDto
        {
            NomeCliente = "Cliente Atualizado",
            TelefoneCliente = "11999999998",
            Itens = new List<ItemPedidoDto>(),
            TipoPedido = TipoPedido.Interno,
            MesaId = Guid.NewGuid(),
            MetodoPagamentoId = Guid.NewGuid(),
            StatusPedidoId = Guid.NewGuid()
        };

        var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("id", Guid.NewGuid().ToString()) }));

        _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(pedidoId))
            .ReturnsAsync((Pedido)null);

        var result = await _updatePedidoService.ExecuteAsync(pedidoId, pedidoDto, userClaims);

        Assert.Null(result.Result);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Pedido não encontrado", result.Message);

        _pedidoRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Pedido>()), Times.Never);
        _transactionManagerMock.Verify(t => t.CommitTransactionAsync(), Times.Never);
        _transactionManagerMock.Verify(t => t.RollbackTransactionAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdatePedido_ShouldReturnValidationError_WhenValidationFails()
    {
        var pedidoId = Guid.NewGuid();
        var pedidoDto = new PedidoDto
        {
            NomeCliente = "Cliente Atualizado",
            TelefoneCliente = "11999999998",
            Itens = new List<ItemPedidoDto>(),
            TipoPedido = TipoPedido.Interno,
            MesaId = Guid.NewGuid(),
            MetodoPagamentoId = Guid.NewGuid(),
            StatusPedidoId = Guid.NewGuid()
        };

        var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("id", Guid.NewGuid().ToString()) }));
        var validationResult = new OperationResult<Pedido> { Message = "Erro de validação", StatusCode = 400 };

        _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(pedidoId))
            .ReturnsAsync(new Pedido());

        _validarPedidosServiceMock.Setup(v => v.ValidarPedido(It.IsAny<PedidoDto>(), It.IsAny<string>()))
            .ReturnsAsync(validationResult);

        var result = await _updatePedidoService.ExecuteAsync(pedidoId, pedidoDto, userClaims);

        Assert.Null(result.Result);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Erro de validação", result.Message);

        _pedidoRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Pedido>()), Times.Never);
        _transactionManagerMock.Verify(t => t.CommitTransactionAsync(), Times.Never);
        _transactionManagerMock.Verify(t => t.RollbackTransactionAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdatePedido_ShouldLogError_WhenExceptionOccurs()
    {
        var pedidoId = Guid.NewGuid();
        var pedidoDto = new PedidoDto
        {
            NomeCliente = "Cliente Atualizado",
            TelefoneCliente = "11999999998",
            Itens = new List<ItemPedidoDto>(),
            TipoPedido = TipoPedido.Interno,
            MesaId = Guid.NewGuid(),
            MetodoPagamentoId = Guid.NewGuid(),
            StatusPedidoId = Guid.NewGuid()
        };

        var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("id", Guid.NewGuid().ToString()) }));

        _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(pedidoId))
            .ThrowsAsync(new Exception("Erro inesperado"));

        var result = await _updatePedidoService.ExecuteAsync(pedidoId, pedidoDto, userClaims);

        Assert.Null(result.Result);
        Assert.Equal(500, result.StatusCode);
        Assert.Equal("Erro inesperado: Erro inesperado", result.Message);
        _transactionManagerMock.Verify(t => t.RollbackTransactionAsync(), Times.Once);
    }
}
