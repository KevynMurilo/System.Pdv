using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Pdv.Application.Services.Mesas;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.Services.Mesas
{
    public class DeleteMesaServiceTests
    {
        private readonly Mock<IMesaRepository> _mockMesaRepository;
        private readonly Mock<ILogger<DeleteMesaService>> _mockLogger;
        private readonly DeleteMesaService _deleteMesaService;

        public DeleteMesaServiceTests()
        {
            _mockMesaRepository = new Mock<IMesaRepository>();
            _mockLogger = new Mock<ILogger<DeleteMesaService>>();
            _deleteMesaService = new DeleteMesaService(_mockMesaRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task DeleteMesa_ShouldReturnNotFound_WhenMesaDoesNotExist()
        {
            var id = Guid.NewGuid();
            _mockMesaRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Mesa)null);

            var result = await _deleteMesaService.DeleteMesa(id);

            Assert.True(result.ServerOn);
            Assert.Equal("Mesa não encontrada", result.Message);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task DeleteMesa_ShouldDeleteMesa_WhenMesaExists()
        {
            var id = Guid.NewGuid();
            var mesa = new Mesa { Id = id };
            _mockMesaRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(mesa);

            var result = await _deleteMesaService.DeleteMesa(id);

            _mockMesaRepository.Verify(repo => repo.DeleteAsync(mesa), Times.Once);
            Assert.True(result.ServerOn);
            Assert.Equal("Mesa deletada com sucesso!", result.Message);
        }

        [Fact]
        public async Task DeleteMesa_ShouldLogError_WhenExceptionIsThrown()
        {
            var id = Guid.NewGuid();
            var exception = new Exception("Test exception");
            _mockMesaRepository.Setup(repo => repo.GetByIdAsync(id)).ThrowsAsync(exception);

            var result = await _deleteMesaService.DeleteMesa(id);

            Assert.False(result.ServerOn);
            Assert.Equal("Erro inesperado: Test exception", result.Message);
            Assert.Equal(500, result.StatusCode);

            _mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Ocorreu um erro ao listar mesas")),
                    It.Is<Exception>(e => e == exception),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                ),
                Times.Once
            );
        }
    }
}
