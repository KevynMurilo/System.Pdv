using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Services.Garcons;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.Services.Garcons
{
    public class CreateGarcomServiceTests
    {
        private readonly Mock<IGarcomRepository> _garcomRepositoryMock;
        private readonly Mock<ILogger<CreateGarcomService>> _loggerMock;
        private readonly CreateGarcomService _createGarcomService;

        public CreateGarcomServiceTests()
        {
            _garcomRepositoryMock = new Mock<IGarcomRepository>();
            _loggerMock = new Mock<ILogger<CreateGarcomService>>();
            _createGarcomService = new CreateGarcomService(_garcomRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateGarcom_ShouldReturnOperationResultWithGarcom_WhenSuccessful()
        {
            var garcomDto = new RegisterGarcomDto
            {
                Nome = "Teste",
                Email = "teste@example.com",
                Password = "senha123"
            };

            _garcomRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Garcom>()))
                .Returns(Task.CompletedTask);

            var result = await _createGarcomService.CreateGarcom(garcomDto);

            Assert.NotNull(result);
            Assert.True(result.ServerOn);
            Assert.NotNull(result.Result);
            Assert.Equal(garcomDto.Nome, result.Result.Nome);
            _garcomRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Garcom>()), Times.Once);
        }

        [Fact]
        public async Task CreateGarcom_ShouldReturnError_WhenExceptionIsThrown()
        {
            var garcomDto = new RegisterGarcomDto { Nome = "João", Email = "joao@example.com", Password = "password123" };

            _garcomRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Garcom>()))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _createGarcomService.CreateGarcom(garcomDto);

            Assert.False(result.ServerOn);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal("Erro inesperado: Database error", result.Message);

            _loggerMock.Verify(logger =>
                logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }
    }
}
