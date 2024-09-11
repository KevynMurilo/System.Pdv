using Moq;
using System.Pdv.Application.UseCase.Clientes;
using System.Pdv.Core.Entities;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.UseCase.Clientes;

public class GetByNameClienteUseCaseTests
{
    private readonly Mock<IClienteRepository> _mockClienteRepository;
    private readonly Mock<ILogger<GetByNameClienteUseCase>> _mockLogger;
    private readonly GetByNameClienteUseCase _getByNameClienteUseCase;

    public GetByNameClienteUseCaseTests()
    {
        _mockClienteRepository = new Mock<IClienteRepository>();
        _mockLogger = new Mock<ILogger<GetByNameClienteUseCase>>();
        _getByNameClienteUseCase = new GetByNameClienteUseCase(_mockClienteRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnClientes_WhenClientesWithNameExist()
    {
        var nome = "Teste";
        var clientes = new List<Cliente>
        {
            new Cliente { Id = Guid.NewGuid(), Nome = "Teste Cliente 1" },
            new Cliente { Id = Guid.NewGuid(), Nome = "Teste Cliente 2" }
        };
        _mockClienteRepository.Setup(repo => repo.GetByNameAsync(nome)).ReturnsAsync(clientes);

        var result = await _getByNameClienteUseCase.ExecuteAsync(nome);

        Assert.NotNull(result);
        Assert.True(result.ServerOn);
        Assert.Equal(clientes, result.Result);
        Assert.Null(result.Message);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenNoClientesWithNameExist()
    {
        var nome = "NomeInexistente";
        _mockClienteRepository.Setup(repo => repo.GetByNameAsync(nome)).ReturnsAsync(new List<Cliente>());

        var result = await _getByNameClienteUseCase.ExecuteAsync(nome);

        Assert.Null(result.Result);
        Assert.Equal("Nenhum cliente com esse nome encontrado", result.Message);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnServerError_WhenExceptionIsThrown()
    {
        var nome = "Teste";
        _mockClienteRepository.Setup(repo => repo.GetByNameAsync(nome))
            .ThrowsAsync(new Exception("Erro de teste"));

        var result = await _getByNameClienteUseCase.ExecuteAsync(nome);

        Assert.False(result.ServerOn);
        Assert.Equal("Erro inesperado: Erro de teste", result.Message);
        Assert.Equal(500, result.StatusCode);
    }
}
