using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Pdv.Application.UseCase.Adicionais;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.UseCase.Adicionais;
public class GetByIdAdicionalServiceTests
{
    private readonly Mock<IAdicionalRepository> _adicionalRepositoryMock;
    private readonly Mock<ILogger<GetByIdAdicionalUseCase>> _loggerMock;
    private readonly GetByIdAdicionalUseCase _getByIdAdicionalService;

    public GetByIdAdicionalServiceTests()
    {
        _adicionalRepositoryMock = new Mock<IAdicionalRepository>();
        _loggerMock = new Mock<ILogger<GetByIdAdicionalUseCase>>();
        _getByIdAdicionalService = new GetByIdAdicionalUseCase(_adicionalRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenAdicionalDoesNotExist()
    {
        var adicionalId = Guid.NewGuid();
        _adicionalRepositoryMock.Setup(repo => repo.GetByIdAsync(adicionalId))
            .ReturnsAsync((ItemAdicional)null);

        var result = await _getByIdAdicionalService.ExecuteAsync(adicionalId);

        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Adicional não encontrado", result.Message);
        _adicionalRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task GetById_ShouldReturnSuccess_WhenAdicionalIsFound()
    {
        var adicionalId = Guid.NewGuid();
        var adicional = new ItemAdicional { Id = adicionalId, Nome = "Adicional Teste", Preco = 10.0m };

        _adicionalRepositoryMock.Setup(repo => repo.GetByIdAsync(adicionalId))
            .ReturnsAsync(adicional);

        var result = await _getByIdAdicionalService.ExecuteAsync(adicionalId);

        Assert.Equal(adicionalId, result.Result.Id);
        Assert.Equal("Adicional Teste", result.Result.Nome);
        Assert.Equal(10.0m, result.Result.Preco);
        Assert.Equal(200, result.StatusCode);
        _adicionalRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task GetById_ShouldLogErrorAndReturnError_WhenExceptionIsThrown()
    {
        var adicionalId = Guid.NewGuid();
        var exception = new Exception("Database error");

        _adicionalRepositoryMock.Setup(repo => repo.GetByIdAsync(adicionalId))
            .ThrowsAsync(exception);

        var result = await _getByIdAdicionalService.ExecuteAsync(adicionalId);

        Assert.False(result.ReqSuccess);
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("Erro inesperado", result.Message);
        _adicionalRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}
