using Moq;
using System.Pdv.Application.UseCase.Clientes;
using System.Pdv.Core.Entities;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.UseCase.Clientes;

public class GetByIdClienteUseCaseTests
{
    private readonly Mock<IClienteRepository> _mockClienteRepository;
    private readonly Mock<ILogger<GetByIdClienteUseCase>> _mockLogger;
    private readonly GetByIdClienteUseCase _getByIdClienteUseCase;

    public GetByIdClienteUseCaseTests()
    {
        _mockClienteRepository = new Mock<IClienteRepository>();
        _mockLogger = new Mock<ILogger<GetByIdClienteUseCase>>();
        _getByIdClienteUseCase = new GetByIdClienteUseCase(_mockClienteRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnCliente_WhenClienteExists()
    {
        var clienteId = Guid.NewGuid();
        var cliente = new Cliente { Id = clienteId, Nome = "Teste Cliente" };
        _mockClienteRepository.Setup(repo => repo.GetByIdAsync(clienteId)).ReturnsAsync(cliente);

        var result = await _getByIdClienteUseCase.ExecuteAsync(clienteId);

        Assert.NotNull(result);
        Assert.True(result.ServerOn);
        Assert.Equal(cliente, result.Result);
        Assert.Null(result.Message);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenClienteDoesNotExist()
    {
        var clienteId = Guid.NewGuid();
        _mockClienteRepository.Setup(repo => repo.GetByIdAsync(clienteId)).ReturnsAsync((Cliente)null);

        var result = await _getByIdClienteUseCase.ExecuteAsync(clienteId);

        Assert.Null(result.Result);
        Assert.Equal("Cliente não encontrado", result.Message);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnServerError_WhenExceptionIsThrown()
    {
        var clienteId = Guid.NewGuid();
        _mockClienteRepository.Setup(repo => repo.GetByIdAsync(clienteId))
            .ThrowsAsync(new Exception("Erro de teste"));

        var result = await _getByIdClienteUseCase.ExecuteAsync(clienteId);

        Assert.False(result.ServerOn);
        Assert.Equal("Erro inesperado: Erro de teste", result.Message);
        Assert.Equal(500, result.StatusCode);
    }
}
