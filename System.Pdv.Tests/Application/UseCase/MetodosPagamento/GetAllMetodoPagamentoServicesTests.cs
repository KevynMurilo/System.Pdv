using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.UseCase.MetodosPagamento;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.UseCase.MetodosPagamento;

public class GetAllMetodoPagamentoServicesTests
{
    private readonly Mock<IMetodoPagamentoRepository> _metodoPagamentoRepositoryMock;
    private readonly Mock<ILogger<GetAllMetodoPagamentoUseCase>> _loggerMock;
    private readonly GetAllMetodoPagamentoUseCase _service;

    public GetAllMetodoPagamentoServicesTests()
    {
        _metodoPagamentoRepositoryMock = new Mock<IMetodoPagamentoRepository>();
        _loggerMock = new Mock<ILogger<GetAllMetodoPagamentoUseCase>>();
        _service = new GetAllMetodoPagamentoUseCase(_metodoPagamentoRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllMetodoPagamento_ShouldReturnNotFound_WhenNoMetodoPagamentoExists()
    {
        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(Enumerable.Empty<MetodoPagamento>());

        var result = await _service.ExecuteAsync();

        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Nenhum método de pagamento encontrado", result.Message);
        _metodoPagamentoRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllMetodoPagamento_ShouldReturnAllMetodosPagamento_WhenMetodosExist()
    {
        var metodosPagamento = new List<MetodoPagamento>
        {
            new MetodoPagamento { Id = Guid.NewGuid(), Nome = "PIX" },
            new MetodoPagamento { Id = Guid.NewGuid(), Nome = "Cartão de Crédito" }
        };
        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(metodosPagamento);

        var result = await _service.ExecuteAsync();

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Null(result.Message);
        Assert.Equal(metodosPagamento, result.Result);
        _metodoPagamentoRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllMetodoPagamento_ShouldLogErrorAndReturnServerError_WhenExceptionIsThrown()
    {
        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetAllAsync()).ThrowsAsync(new Exception("Test exception"));

        var result = await _service.ExecuteAsync();

        Assert.NotNull(result);
        Assert.False(result.ServerOn);
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("Erro inesperado:", result.Message);
        _metodoPagamentoRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }
}
