﻿using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.UseCase.Usuarios;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.UseCase.Usuarios;
public class GetAllUsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<ILogger<GetAllUsuarioUseCase>> _loggerMock;
    private readonly GetAllUsuarioUseCase _service;

    public GetAllUsuarioServiceTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _loggerMock = new Mock<ILogger<GetAllUsuarioUseCase>>();
        _service = new GetAllUsuarioUseCase(_usuarioRepositoryMock.Object, _loggerMock.Object);   
    }

    [Fact]
    public async Task GetAllUsuario_ReturnsUsuarios_WhenNoUsuariosExist()
    {
        _usuarioRepositoryMock.Setup(repo => repo.GetAllAsync(1, 10)).ReturnsAsync(new List<Usuario>());
        
        var result = await _service.ExecuteAsync(1, 10);

        Assert.True(result.ReqSuccess);
        Assert.Equal("Nenhum usuário encontrado", result.Message);
        Assert.Equal(404, result.StatusCode);
        _usuarioRepositoryMock.Verify(repo => repo.GetAllAsync(1, 10), Times.Once);
    }

    [Fact]
    public async Task GetAllUsuario_ReturnsMesas_WhenMesasExist()
    {
        var usuarios = new List<Usuario>
        {
            new Usuario { Id = Guid.NewGuid(), Nome = "User1", Email = "Email1@example.com", PasswordHash = "123" },
            new Usuario { Id = Guid.NewGuid(), Nome = "User2", Email = "Email2@example.com", PasswordHash = "123" },
        };

        _usuarioRepositoryMock.Setup(repo => repo.GetAllAsync(1, 10)).ReturnsAsync(usuarios);

        var result = await _service.ExecuteAsync(1, 10);

        Assert.True(result.ReqSuccess);
        Assert.Equal(usuarios, result.Result.ToList());
        _usuarioRepositoryMock.Verify(repo => repo.GetAllAsync(1, 10), Times.Once);
    }

    [Fact]
    public async Task GetAllUsuarios_LogsError_WhenExceptionIsThrown()
    {
        var exception = new Exception("Database failure");
        _usuarioRepositoryMock.Setup(repo => repo.GetAllAsync(1, 10)).ThrowsAsync(exception);

        var result = await _service.ExecuteAsync(1, 10);

        Assert.False(result.ReqSuccess);
        Assert.Equal("Erro inesperado: Database failure", result.Message);
        Assert.Equal(500, result.StatusCode);
        _usuarioRepositoryMock.Verify(repo => repo.GetAllAsync(1, 10), Times.Once);
    }
}
