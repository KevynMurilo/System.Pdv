﻿using Moq;
using Microsoft.Extensions.Logging;
using System.Pdv.Application.Services.StatusPedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;
using System.Threading.Tasks;
using System;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Common;

namespace System.Pdv.Tests.Services.StatusPedidos
{
    public class UpdateStatusPedidoServiceTests
    {
        private readonly Mock<IStatusPedidoRepository> _statusPedidoRepositoryMock;
        private readonly Mock<ILogger<UpdateStatusPedidoService>> _loggerMock;
        private readonly UpdateStatusPedidoService _service;

        public UpdateStatusPedidoServiceTests()
        {
            _statusPedidoRepositoryMock = new Mock<IStatusPedidoRepository>();
            _loggerMock = new Mock<ILogger<UpdateStatusPedidoService>>();
            _service = new UpdateStatusPedidoService(_statusPedidoRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnNotFound_WhenStatusPedidoDoesNotExist()
        {
            var id = Guid.NewGuid();
            var statusPedidoDto = new StatusPedidoDto { Status = "Delivered" };

            _statusPedidoRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync((StatusPedido)null);

            var result = await _service.ExecuteAsync(id, statusPedidoDto);

            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Status de pedido não encontrado", result.Message);
            _statusPedidoRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnConflict_WhenStatusPedidoAlreadyExists()
        {
            var id = Guid.NewGuid();
            var statusPedidoDto = new StatusPedidoDto { Status = "Delivered" };
            var existingStatusPedido = new StatusPedido { Id = id, Status = "PENDING" };

            _statusPedidoRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(existingStatusPedido);

            _statusPedidoRepositoryMock.Setup(repo => repo.GetByNameAsync(statusPedidoDto.Status))
                .ReturnsAsync(new StatusPedido { Status = "DELIVERED" });

            var result = await _service.ExecuteAsync(id, statusPedidoDto);

            Assert.Equal(409, result.StatusCode);
            Assert.Equal("Status de pedido já registrado", result.Message);
            _statusPedidoRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
            _statusPedidoRepositoryMock.Verify(repo => repo.GetByNameAsync(statusPedidoDto.Status), Times.Once);
        }


        [Fact]
        public async Task ExecuteAsync_ShouldUpdateStatusPedido_WhenValidRequest()
        {
            var id = Guid.NewGuid();
            var statusPedidoDto = new StatusPedidoDto { Status = "Delivered" };
            var statusPedido = new StatusPedido { Id = id, Status = "Pending" };

            _statusPedidoRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(statusPedido);

            _statusPedidoRepositoryMock.Setup(repo => repo.GetByNameAsync(statusPedidoDto.Status))
                .ReturnsAsync((StatusPedido)null);

            _statusPedidoRepositoryMock.Setup(repo => repo.UpdateAsync(statusPedido))
                .Returns(Task.CompletedTask);

            var result = await _service.ExecuteAsync(id, statusPedidoDto);

            Assert.Equal("DELIVERED", statusPedido.Status);
            Assert.Equal(statusPedido, result.Result);
            Assert.Equal("Status de pedido atualizado com sucesso", result.Message);
            _statusPedidoRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
            _statusPedidoRepositoryMock.Verify(repo => repo.GetByNameAsync(statusPedidoDto.Status), Times.Once);
            _statusPedidoRepositoryMock.Verify(repo => repo.UpdateAsync(statusPedido), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnServerError_WhenExceptionIsThrown()
        {
            var id = Guid.NewGuid();
            var statusPedidoDto = new StatusPedidoDto { Status = "Delivered" };

            _statusPedidoRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _service.ExecuteAsync(id, statusPedidoDto);

            Assert.False(result.ServerOn);
            Assert.Equal(500, result.StatusCode);
            Assert.Contains("Erro inesperado", result.Message);
            _statusPedidoRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        }
    }
}
