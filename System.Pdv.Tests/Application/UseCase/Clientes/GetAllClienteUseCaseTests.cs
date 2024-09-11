using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.UseCase.Clientes;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.UseCase.Clientes;

public class GetAllClienteUseCaseTests
{
    private readonly Mock<IClienteRepository> _repositoryMock;
    private readonly Mock<ILogger<GetAllClienteUseCase>> _loggerMock;
    private readonly GetAllClienteUseCase _clienteUseCase;

    public GetAllClienteUseCaseTests()
    {
        _repositoryMock = new Mock<IClienteRepository>();
        _loggerMock = new Mock<ILogger<GetAllClienteUseCase>>();
        _clienteUseCase = new GetAllClienteUseCase(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]

    public async Task GetAllClientes_ShouldReturnNotFound_WhenNoClientesAreFound()
    {
        _repositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new List<Cliente>());

        var result = await _clienteUseCase.ExecuteAsync(1, 10);

        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Nenhum cliente encontrado", result.Message);
        Assert.True(result.ServerOn);
        _repositoryMock.Verify(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetAllClientes_ShouldReturnSuccess_WhenClientesAreFound()
    {
        var clientes = new List<Cliente>
        {
            new Cliente { Id = Guid.NewGuid(), Nome = "Cliente 1", Telefone = "99999999999" },
            new Cliente { Id = Guid.NewGuid(), Nome = "Cliente 2", Telefone = "99999999999" }
        };

        _repositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(clientes);

        var result = await _clienteUseCase.ExecuteAsync(1, 10);

        Assert.NotNull(result);
        Assert.Equal(clientes, result.Result);
        Assert.Equal(200, result.StatusCode);
        Assert.True(result.ServerOn);
        _repositoryMock.Verify(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetAllClientes_ShouldLogErrorAndReturnError_WhenExceptionIsThrown()
    {
        var exception = new Exception("Database error");

        _repositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(exception);

        var result = await _clienteUseCase.ExecuteAsync(1, 10);

        Assert.False(result.ServerOn);
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("Erro inesperado", result.Message);
        _repositoryMock.Verify(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }
}
