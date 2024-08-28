﻿using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.Services.Mesas;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

public class GetMesaByIdServiceTests
{
    private readonly Mock<IMesaRepository> _mesaRepositoryMock;
    private readonly Mock<ILogger<GetMesaByIdService>> _loggerMock;
    private readonly GetMesaByIdService _service;

    public GetMesaByIdServiceTests()
    {
        _mesaRepositoryMock = new Mock<IMesaRepository>();
        _loggerMock = new Mock<ILogger<GetMesaByIdService>>();
        _service = new GetMesaByIdService(_mesaRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetById_MesaExists_ReturnsMesa()
    {
        var mesaId = Guid.NewGuid();
        var mesa = new Mesa { Id = mesaId };
        _mesaRepositoryMock.Setup(repo => repo.GetByIdAsync(mesaId))
            .ReturnsAsync(mesa);

        var result = await _service.GetById(mesaId);

        Assert.NotNull(result);
        Assert.Equal(mesa, result.Result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task GetById_MesaNotFound_ReturnsNotFound()
    {
        var mesaId = Guid.NewGuid();
        _mesaRepositoryMock.Setup(repo => repo.GetByIdAsync(mesaId))
            .ReturnsAsync((Mesa)null);

        var result = await _service.GetById(mesaId);

        Assert.NotNull(result);
        Assert.Null(result.Result);
        Assert.Equal("Mesa não encontrada", result.Message);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task GetById_ExceptionThrown_LogsErrorAndReturnsServerError()
    {
        var mesaId = Guid.NewGuid();
        var exception = new Exception("Database failure");
        _mesaRepositoryMock.Setup(repo => repo.GetByIdAsync(mesaId))
            .ThrowsAsync(exception);

        var result = await _service.GetById(mesaId);

        Assert.NotNull(result);
        Assert.Null(result.Result);
        Assert.Equal("Erro inesperado: Database failure", result.Message);
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
