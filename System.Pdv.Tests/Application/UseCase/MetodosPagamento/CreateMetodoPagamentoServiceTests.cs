﻿using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.UseCase.MetodosPagamento;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.UseCase.MetodosPagamento;

public class CreateMetodoPagamentoServiceTests
{
    private readonly Mock<IMetodoPagamentoRepository> _metodoPagamentoRepositoryMock;
    private readonly Mock<ILogger<CreateMetodoPagamentoUseCase>> _loggerMock;
    private readonly CreateMetodoPagamentoUseCase _service;

    public CreateMetodoPagamentoServiceTests()
    {
        _metodoPagamentoRepositoryMock = new Mock<IMetodoPagamentoRepository>();
        _loggerMock = new Mock<ILogger<CreateMetodoPagamentoUseCase>>();
        _service = new CreateMetodoPagamentoUseCase(_metodoPagamentoRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task CreateMetodoPagamento_ShouldReturnConflict_WhenMetodoPagamentoAlreadyExists()
    {
        var metodoPagamentoDto = new MetodoPagamentoDto { Nome = "Cartão de Crédito" };
        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(new MetodoPagamento());

        var result = await _service.ExecuteAsync(metodoPagamentoDto);

        Assert.NotNull(result);
        Assert.Equal(409, result.StatusCode);
        Assert.Equal("Método de pagamento já cadastrado", result.Message);
        Assert.Null(result.Result);
        _metodoPagamentoRepositoryMock.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CreateMetodoPagamento_ShouldReturnSuccess_WhenMetodoPagamentoIsCreatedSuccessfully()
    {
        var metodoPagamentoDto = new MetodoPagamentoDto { Nome = "Cartão de Débito" };
        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((MetodoPagamento)null);
        _metodoPagamentoRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<MetodoPagamento>())).Returns(Task.CompletedTask);

        var result = await _service.ExecuteAsync(metodoPagamentoDto);

        Assert.NotNull(result);
        Assert.Null(result.Message);
        Assert.Equal("CARTÃO DE DÉBITO", result.Result.Nome);
        Assert.True(result.ReqSuccess);
        Assert.Equal(200, result.StatusCode);
        _metodoPagamentoRepositoryMock.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
        _metodoPagamentoRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<MetodoPagamento>()), Times.Once);
    }

    [Fact]
    public async Task CreateMetodoPagamento_ShouldLogErrorAndReturnServerError_WhenExceptionIsThrown()
    {
        var metodoPagamentoDto = new MetodoPagamentoDto { Nome = "Pix" };
        _metodoPagamentoRepositoryMock.Setup(repo => repo.GetByNameAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Test exception"));

        var result = await _service.ExecuteAsync(metodoPagamentoDto);

        Assert.NotNull(result);
        Assert.False(result.ReqSuccess);
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("Erro inesperado:", result.Message);
        _metodoPagamentoRepositoryMock.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.Once);
    }
}
