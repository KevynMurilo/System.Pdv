using Moq;
using System;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.UseCase.Mesas;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
using System.Pdv.Core.Interfaces;
using Xunit;
using Microsoft.Extensions.Logging;

namespace System.Pdv.Tests.UseCase.Mesas;
public class UpdateMesaServiceTests
{
    private readonly Mock<IMesaRepository> _mesaRepositoryMock;
    private readonly Mock<ILogger<UpdateMesaUseCase>> _loggerMock;
    private readonly UpdateMesaUseCase _updateMesaService;

    public UpdateMesaServiceTests()
    {
        _mesaRepositoryMock = new Mock<IMesaRepository>();
        _loggerMock = new Mock<ILogger<UpdateMesaUseCase>>();
        _updateMesaService = new UpdateMesaUseCase(_mesaRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task UpdateMesa_ShouldReturnNotFound_WhenMesaDoesNotExist()
    {
        var mesaId = Guid.NewGuid();
        _mesaRepositoryMock.Setup(repo => repo.GetByIdAsync(mesaId))
            .ReturnsAsync((Mesa)null);

        var mesaDto = new UpdateMesaDto { Numero = 1, Status = StatusMesa.Ocupada };

        var result = await _updateMesaService.ExecuteAsync(mesaId, mesaDto);

        Assert.True(result.ReqSuccess);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Mesa não encontrada", result.Message);
        _mesaRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task UpdateMesa_ShouldUpdateMesa_WhenMesaExists()
    {
        var mesaId = Guid.NewGuid();
        var existingMesa = new Mesa { Id = mesaId, Numero = 1, Status = StatusMesa.Livre };

        _mesaRepositoryMock.Setup(repo => repo.GetByIdAsync(mesaId)).ReturnsAsync(existingMesa);

        var mesaDto = new UpdateMesaDto { Numero = 2, Status = StatusMesa.Ocupada };

        var result = await _updateMesaService.ExecuteAsync(mesaId, mesaDto);

        Assert.True(result.ReqSuccess);
        Assert.Equal(2, result.Result.Numero);
        Assert.Equal(StatusMesa.Ocupada, result.Result.Status);
        _mesaRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mesaRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Mesa>()), Times.Once);
    }

    [Fact]
    public async Task UpdateMesa_ShouldLogError_WhenExceptionIsThrown()
    {
        var mesaId = Guid.NewGuid();
        var mesaDto = new UpdateMesaDto { Numero = 1, Status = StatusMesa.Ocupada };
        var exception = new Exception("Database error");

        _mesaRepositoryMock.Setup(repo => repo.GetByIdAsync(mesaId)).ThrowsAsync(exception);

        var result = await _updateMesaService.ExecuteAsync(mesaId, mesaDto);

        Assert.False(result.ReqSuccess);
        Assert.Equal(500, result.StatusCode);
        Assert.StartsWith("Erro inesperado:", result.Message);
        _mesaRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}
