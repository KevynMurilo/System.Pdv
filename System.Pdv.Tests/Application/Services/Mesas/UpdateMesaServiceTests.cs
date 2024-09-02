using Moq;
using System;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Services.Mesas;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
using System.Pdv.Core.Interfaces;
using Xunit;
using Microsoft.Extensions.Logging;

namespace System.Pdv.Tests.Services.Mesas
{
    public class UpdateMesaServiceTests
    {
        private readonly Mock<IMesaRepository> _mesaRepositoryMock;
        private readonly Mock<ILogger<UpdateMesaService>> _loggerMock;
        private readonly UpdateMesaService _updateMesaService;

        public UpdateMesaServiceTests()
        {
            _mesaRepositoryMock = new Mock<IMesaRepository>();
            _loggerMock = new Mock<ILogger<UpdateMesaService>>();
            _updateMesaService = new UpdateMesaService(_mesaRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task UpdateMesa_ShouldReturnNotFound_WhenMesaDoesNotExist()
        {
            var mesaId = Guid.NewGuid();
            _mesaRepositoryMock.Setup(repo => repo.GetByIdAsync(mesaId))
                .ReturnsAsync((Mesa)null);

            var mesaDto = new MesaDto { Numero = 1, Status = StatusMesa.Ocupada };

            var result = await _updateMesaService.UpdateMesa(mesaId, mesaDto);

            Assert.True(result.ServerOn);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Mesa não encontrada", result.Message);
        }

        [Fact]
        public async Task UpdateMesa_ShouldUpdateMesa_WhenMesaExists()
        {
            var mesaId = Guid.NewGuid();
            var existingMesa = new Mesa { Id = mesaId, Numero = 1, Status = StatusMesa.Livre };

            _mesaRepositoryMock.Setup(repo => repo.GetByIdAsync(mesaId)).ReturnsAsync(existingMesa);

            var mesaDto = new MesaDto { Numero = 2, Status = StatusMesa.Ocupada };

            var result = await _updateMesaService.UpdateMesa(mesaId, mesaDto);

            Assert.True(result.ServerOn);
            Assert.Equal(2, result.Result.Numero);
            Assert.Equal(StatusMesa.Ocupada, result.Result.Status);
            _mesaRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Mesa>()), Times.Once);
        }

        [Fact]
        public async Task UpdateMesa_ShouldLogError_WhenExceptionIsThrown()
        {
            var mesaId = Guid.NewGuid();
            var mesaDto = new MesaDto { Numero = 1, Status = StatusMesa.Ocupada };
            var exception = new Exception("Database error");

            _mesaRepositoryMock.Setup(repo => repo.GetByIdAsync(mesaId)).ThrowsAsync(exception);

            var result = await _updateMesaService.UpdateMesa(mesaId, mesaDto);

            Assert.False(result.ServerOn);
            Assert.Equal(500, result.StatusCode);
            Assert.StartsWith("Erro inesperado:", result.Message);
        }
    }
}
