﻿using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.UseCase.Usuarios;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.UseCase.Usuarios;
public class DeleteUsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<ILogger<DeleteUsuarioUseCase>> _loggerMock;
    private readonly DeleteUsuarioUseCase _service;

    public DeleteUsuarioServiceTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _loggerMock = new Mock<ILogger<DeleteUsuarioUseCase>>();
        _service = new DeleteUsuarioUseCase(_usuarioRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task DeleteUsuario_ShouldDeleteUsuario_WhenUsuarioExists()
    {
        var usuarioId = Guid.NewGuid();
        var usuario = new Usuario
        {
            Nome = "kevyn",
            Email = "kevyn@email.com",
            PasswordHash = "dsads8a7d9sa123sGasd"
        };

        _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(usuarioId))
            .ReturnsAsync(usuario);

        var result = await _service.ExecuteAsync(usuarioId);

        Assert.NotNull(result);
        Assert.True(result.ReqSuccess);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("Usuário deletado com sucesso", result.Message);
        _usuarioRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _usuarioRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Usuario>()), Times.Once);
    }


    [Fact]
    public async Task DeleteUsuario_ShouldReturnNotFound_WhenUsuarioDoesNotExist()
    {
        var usuarioId = Guid.NewGuid();
        _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(usuarioId))
            .ReturnsAsync((Usuario)null);

        var result = await _service.ExecuteAsync(usuarioId);

        Assert.NotNull(result);
        Assert.True(result.ReqSuccess);
        Assert.Equal("Usuário não encontrado", result.Message);
        Assert.Equal(404, result.StatusCode);
        _usuarioRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _usuarioRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Usuario>()), Times.Never);
    }

    [Fact]
    public async Task DeleteUsuario_ShouldLogError_WhenExceptionIsThrown()
    {
        var usuarioId = Guid.NewGuid();
        var exception = new Exception("Database error");

        _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(usuarioId)).ThrowsAsync(exception);

        var result = await _service.ExecuteAsync(usuarioId);

        Assert.NotNull(result);
        Assert.False(result.ReqSuccess);
        Assert.Equal("Erro inesperado: Database error", result.Message);
        Assert.Equal(500, result.StatusCode);
        _usuarioRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _usuarioRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Usuario>()), Times.Never);
    }
}
