using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Pdv.Application.Services.Mesas;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using System.Pdv.Core.Enums;

namespace System.Pdv.Application.Tests.Services.Mesas;

public class GetAllServicesTests
{
    private readonly Mock<IMesaRepository> _mesaRepositoryMock;
    private readonly Mock<ILogger<GetAllService>> _loggerMock;
    private readonly GetAllService _service;

    public GetAllServicesTests()
    {
        _mesaRepositoryMock = new Mock<IMesaRepository>();
        _loggerMock = new Mock<ILogger<GetAllService>>();
        _service = new GetAllService(_mesaRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllMesas_ReturnsNoMesas_WhenNoMesasExist()
    {
        _mesaRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Mesa>());

        var result = await _service.GetAllMesas();

        Assert.True(result.Status);
        Assert.Equal("Nenhuma mesa registrada", result.Message);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task GetAllMesas_ReturnsMesas_WhenMesasExist()
    {
        var mesas = new List<Mesa>
        {
            new Mesa { Id = Guid.NewGuid(), Numero = 1, Status = StatusMesa.Livre },
            new Mesa { Id = Guid.NewGuid(), Numero = 2, Status = StatusMesa.Ocupada }
        };
        _mesaRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(mesas);

        var result = await _service.GetAllMesas();

        Assert.True(result.Status);
        Assert.Equal(mesas, result.Result.ToList());
    }

    [Fact]
    public async Task GetAllMesas_LogsError_WhenExceptionIsThrown()
    {
        var exception = new Exception("Erro de teste");
        _mesaRepositoryMock.Setup(repo => repo.GetAllAsync()).ThrowsAsync(exception);

        var result = await _service.GetAllMesas();

        Assert.False(result.Status);
        Assert.Equal("Erro inesperado: Erro de teste", result.Message);
        Assert.Equal(500, result.StatusCode);

        _loggerMock.Verify(logger =>
            logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.Is<Exception>(ex => ex == exception),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }
}
