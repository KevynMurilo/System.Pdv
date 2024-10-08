﻿using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.UseCase.Usuarios;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.UseCase.Usuarios;
public class GetByIdUsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<ILogger<GetByIdUsuarioUseCase>> _loggerMock;
    private readonly GetByIdUsuarioUseCase _service;

    public GetByIdUsuarioServiceTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _loggerMock = new Mock<ILogger<GetByIdUsuarioUseCase>>();
        _service = new GetByIdUsuarioUseCase(_usuarioRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetById_UsuarioExists_ReturnsUsuario()
    {
        var usuarioId = Guid.NewGuid();
        var usuario = new Usuario { Id = usuarioId };
        _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(usuarioId))
            .ReturnsAsync(usuario);

        var result = await _service.ExecuteAsync(usuarioId);

        Assert.NotNull(result);
        Assert.Equal(usuario, result.Result);
        Assert.Equal(200, result.StatusCode);
        _usuarioRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task GetById_UsuarioNotFound_ReturnsNotFound()
    {
        var usuarioId = Guid.NewGuid();
        _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(usuarioId))
            .ReturnsAsync((Usuario)null);

        var result = await _service.ExecuteAsync(usuarioId);

        Assert.NotNull(result);
        Assert.Null(result.Result);
        Assert.Equal("Usuário não encontrado", result.Message);
        Assert.Equal(404, result.StatusCode);
        _usuarioRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task GetById_ExceptionThrown_LogsErrorAndReturnsServerError()
    {
        var usuarioId = Guid.NewGuid();
        var exception = new Exception("Database failure");
        _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(usuarioId))
            .ThrowsAsync(exception);

        var result = await _service.ExecuteAsync(usuarioId);

        Assert.NotNull(result);
        Assert.Null(result.Result);
        Assert.Equal("Erro inesperado: Database failure", result.Message);
        Assert.Equal(500, result.StatusCode);
        _usuarioRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}
