using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Services.MetodosPagamento;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.Services.MetodosPagamento;

public class UpdateMetodoPagamentoServiceTests
{
    private readonly Mock<IMetodoPagamentoRepository> _metodoPagamentoRepositoryMock;
    private readonly Mock<ILogger<UpdateMetodoPagamentoService>> _loggerMock;
    private readonly UpdateMetodoPagamentoService _service;

    public UpdateMetodoPagamentoServiceTests()
    {
        _metodoPagamentoRepositoryMock = new Mock<IMetodoPagamentoRepository>();
        _loggerMock = new Mock<ILogger<UpdateMetodoPagamentoService>>();
        _service = new UpdateMetodoPagamentoService(_metodoPagamentoRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task UpdateMetodoPagamento_ShouldReturnNotFound_WhenMetodoPagamentoDoesNotExist()
    {
        var metodoPagamentoId = Guid.NewGuid();
        var metodoPagamentoDto = new MetodoPagamentoDto { Nome = "Transferência" };
        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetByIdAsync(metodoPagamentoId))
            .ReturnsAsync((MetodoPagamento)null);

        var result = await _service.UpdateMetodoPagamento(metodoPagamentoId, metodoPagamentoDto);

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Método de pagamento não encontrado", result.Message);
        Assert.Null(result.Result);
    }

    [Fact]
    public async Task UpdateMetodoPagamento_ShouldReturnConflict_WhenMetodoPagamentoNameAlreadyExists()
    {
        var metodoPagamentoId = Guid.NewGuid();
        var metodoPagamentoDto = new MetodoPagamentoDto { Nome = "Cartão de Crédito" };
        var existingMetodoPagamento = new MetodoPagamento { Id = Guid.NewGuid(), Nome = "Cartão de Crédito" };

        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetByIdAsync(metodoPagamentoId))
            .ReturnsAsync(new MetodoPagamento { Id = metodoPagamentoId, Nome = "PIX" });

        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetByNameAsync(metodoPagamentoDto.Nome))
            .ReturnsAsync(existingMetodoPagamento);

        var result = await _service.UpdateMetodoPagamento(metodoPagamentoId, metodoPagamentoDto);

        Assert.NotNull(result);
        Assert.Equal(409, result.StatusCode);
        Assert.Equal("Método de pagamento já cadastrado", result.Message);
        Assert.Null(result.Result);
    }

    [Fact]
    public async Task UpdateMetodoPagamento_ShouldUpdateMetodoPagamento_WhenValidDataProvided()
    {
        var metodoPagamentoId = Guid.NewGuid();
        var metodoPagamentoDto = new MetodoPagamentoDto { Nome = "Boleto" };
        var metodoPagamento = new MetodoPagamento { Id = metodoPagamentoId, Nome = "PIX" };

        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetByIdAsync(metodoPagamentoId))
            .ReturnsAsync(metodoPagamento);

        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetByNameAsync(metodoPagamentoDto.Nome))
            .ReturnsAsync((MetodoPagamento)null);

        var result = await _service.UpdateMetodoPagamento(metodoPagamentoId, metodoPagamentoDto);

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Null(result.Message);
        Assert.Equal(metodoPagamentoId, result.Result.Id);
        Assert.Equal("BOLETO", result.Result.Nome);
    }

    [Fact]
    public async Task UpdateMetodoPagamento_ShouldLogErrorAndReturnServerError_WhenExceptionIsThrown()
    {
        var metodoPagamentoId = Guid.NewGuid();
        var metodoPagamentoDto = new MetodoPagamentoDto { Nome = "Débito Automático" };

        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetByIdAsync(metodoPagamentoId))
            .ThrowsAsync(new Exception("Test exception"));

        var result = await _service.UpdateMetodoPagamento(metodoPagamentoId, metodoPagamentoDto);

        Assert.NotNull(result);
        Assert.False(result.ServerOn);
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("Erro inesperado:", result.Message);
    }
}
