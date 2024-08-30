using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Pdv.Application.Services.MesaService;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.Services.Mesas
{
    public class CreateMesaServiceTests
    {
        private readonly Mock<IMesaRepository> _mesaRepositoryMock;
        private readonly Mock<ILogger<CreateMesaService>> _loggerMock;
        private readonly CreateMesaService _createMesaService;

        public CreateMesaServiceTests()
        {
            _mesaRepositoryMock = new Mock<IMesaRepository>();
            _loggerMock = new Mock<ILogger<CreateMesaService>>();
            _createMesaService = new CreateMesaService(_mesaRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateMesa_ShouldReturnConflict_WhenMesaAlreadyExists()
        {
            var mesaDto = new MesaDto { Numero = 1 };
            var existingMesa = new Mesa { Numero = 1 };

            _mesaRepositoryMock.Setup(repo => repo.GetByNumberAsync(It.IsAny<int>()))
                .ReturnsAsync(existingMesa);

            var result = await _createMesaService.CreateMesa(mesaDto);

            Assert.Equal(409, result.StatusCode);
            Assert.Equal("Mesa já registrada", result.Message);
        }

        [Fact]
        public async Task CreateMesa_ShouldReturnSuccess_WhenMesaIsCreated()
        {
            var mesaDto = new MesaDto { Numero = 1 };
            Mesa mesaCreated = null;

            _mesaRepositoryMock.Setup(repo => repo.GetByNumberAsync(It.IsAny<int>()))
                .ReturnsAsync((Mesa)null);

            _mesaRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Mesa>()))
                .ReturnsAsync((Mesa mesa) =>
                {
                    mesaCreated = mesa;
                    return mesa;
                });

            var result = await _createMesaService.CreateMesa(mesaDto);

            Assert.NotNull(result.Result);
            Assert.Equal(mesaDto.Numero, result.Result.Numero);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task CreateMesa_ShouldReturnError_WhenExceptionIsThrown()
        {
            var mesaDto = new MesaDto { Numero = 1 };

            _mesaRepositoryMock.Setup(repo => repo.GetByNumberAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _createMesaService.CreateMesa(mesaDto);

            Assert.False(result.ServerOn);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal("Erro inesperado: Database error", result.Message);
        }
    }
}
