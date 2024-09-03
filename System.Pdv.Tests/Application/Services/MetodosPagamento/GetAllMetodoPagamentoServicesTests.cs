using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.Services.MetodosPagamento;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.Services.MetodosPagamento;

public class GetAllMetodoPagamentoServicesTests
{
    private readonly Mock<IMetodoPagamentoRepository> _metodoPagamentoRepositoryMock;
    private readonly Mock<ILogger<GetAllMetodoPagamentoServices>> _loggerMock;
    private readonly GetAllMetodoPagamentoServices _service;

    public GetAllMetodoPagamentoServicesTests()
    {
        _metodoPagamentoRepositoryMock = new Mock<IMetodoPagamentoRepository>();
        _loggerMock = new Mock<ILogger<GetAllMetodoPagamentoServices>>();
        _service = new GetAllMetodoPagamentoServices(_metodoPagamentoRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllMetodoPagamento_ShouldReturnNotFound_WhenNoMetodoPagamentoExists()
    {
        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(Enumerable.Empty<MetodoPagamento>());

        var result = await _service.GetAllMetodoPagamento();

        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Nenhum método de pagamento encontrado", result.Message);
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

        var result = await _service.GetAllMetodoPagamento();

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Null(result.Message);
        Assert.Equal(metodosPagamento, result.Result);
    }

    [Fact]
    public async Task GetAllMetodoPagamento_ShouldLogErrorAndReturnServerError_WhenExceptionIsThrown()
    {
        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetAllAsync()).ThrowsAsync(new Exception("Test exception"));

        var result = await _service.GetAllMetodoPagamento();

        Assert.NotNull(result);
        Assert.False(result.ServerOn);
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("Erro inesperado:", result.Message);

    }
}
