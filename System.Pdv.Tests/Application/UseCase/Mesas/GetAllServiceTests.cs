﻿using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Pdv.Application.UseCase.Mesas;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using System.Pdv.Core.Enums;

namespace System.Pdv.Application.Tests.UseCase.Mesas;
public class GetAllServicesTests
{
    private readonly Mock<IMesaRepository> _mesaRepositoryMock;
    private readonly Mock<ILogger<GetAllMesaUseCase>> _loggerMock;
    private readonly GetAllMesaUseCase _service;

    public GetAllServicesTests()
    {
        _mesaRepositoryMock = new Mock<IMesaRepository>();
        _loggerMock = new Mock<ILogger<GetAllMesaUseCase>>();
        _service = new GetAllMesaUseCase(_mesaRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllMesas_ReturnsNoMesas_WhenNoMesasExist()
    {
        _mesaRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Mesa>());

        var result = await _service.ExecuteAsync();

        Assert.True(result.ReqSuccess);
        Assert.Equal("Nenhuma mesa registrada", result.Message);
        Assert.Equal(404, result.StatusCode);
        _mesaRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
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

        var result = await _service.ExecuteAsync();

        Assert.NotNull(result);
        Assert.True(result.ReqSuccess);
        Assert.Equal(mesas, result.Result);
        Assert.Equal(200, result.StatusCode);
        _mesaRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllMesas_LogsError_WhenExceptionIsThrown()
    {
        var exception = new Exception("Erro de teste");
        _mesaRepositoryMock.Setup(repo => repo.GetAllAsync()).ThrowsAsync(exception);

        var result = await _service.ExecuteAsync();

        Assert.False(result.ReqSuccess);
        Assert.Equal("Erro inesperado: Erro de teste", result.Message);
        Assert.Equal(500, result.StatusCode);
        _mesaRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }
}
