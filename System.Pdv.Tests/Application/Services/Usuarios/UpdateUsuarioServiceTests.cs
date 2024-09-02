﻿using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Services.Usuarios;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.Services.Usuarios;

public class UpdateUsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IRoleRepository> _roleRepositoryMock;
    private readonly Mock<ILogger<UpdateUsuarioService>> _loggerMock;
    private readonly UpdateUsuarioService _service;

    public UpdateUsuarioServiceTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _roleRepositoryMock = new Mock<IRoleRepository>();
        _loggerMock = new Mock<ILogger<UpdateUsuarioService>>();
        _service = new UpdateUsuarioService(_usuarioRepositoryMock.Object, _roleRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task UpdateUsuario_ShouldUpdateUsuarioSuccessfully_WhenValidDataProvided()
    {
        var usuarioId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        var existingUsuario = new Usuario { Id = usuarioId, RoleId = roleId, Nome = "Nome", Email = "existing@email.com", PasswordHash = "312mk312kosSmdiosp" };
        var usuarioDto = new UsuarioDto { Nome = "Teste", Email = "teste@example.com", Password = "senha123", RoleId = roleId };
        var existingRole = new Role { Id = roleId, Nome = "Usuario", Descricao = "descricao" };

        _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(usuarioId))
            .ReturnsAsync(existingUsuario);

        _usuarioRepositoryMock.Setup(repo => repo.GetByEmail(usuarioDto.Email))
             .ReturnsAsync((Usuario)null);

        _roleRepositoryMock.Setup(repo => repo.GetByIdAsync(roleId))
            .ReturnsAsync(existingRole);

        var result = await _service.UpdateUsuario(usuarioId, usuarioDto);

        Assert.NotNull(result);
        Assert.True(result.ServerOn);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task UpdateUsuario_ShouldReturnNotFound_WhenUsuarioDoesNotExist()
    {
        var usuarioId = Guid.NewGuid();

        _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(usuarioId))
            .ReturnsAsync((Usuario)null);

        var usuarioDto = new UsuarioDto { Nome = "nome", Email = "email@email.com", Password = "12345678" };

        var result = await _service.UpdateUsuario(usuarioId, usuarioDto);

        Assert.NotNull(result);
        Assert.True(result.ServerOn);
        Assert.Equal("Usuário não encontrado", result.Message);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task UpdateUsuario_ShouldReturn_NotFoundRole()
    {
        var usuarioId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        var existingUsuario = new Usuario { Id = usuarioId, RoleId = roleId, Nome = "Nome", Email = "existing@email.com", PasswordHash = "312mk312kosSmdiosp" };
        var usuarioDto = new UsuarioDto { Nome = "Teste", Email = "teste@example.com", Password = "senha123", RoleId = roleId };

        _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(usuarioId))
            .ReturnsAsync(existingUsuario);

        _usuarioRepositoryMock.Setup(repo => repo.GetByEmail(usuarioDto.Email))
             .ReturnsAsync((Usuario)null);

        _roleRepositoryMock.Setup(repo => repo.GetByIdAsync(roleId))
            .ReturnsAsync((Role)null);

        var result = await _service.UpdateUsuario(usuarioId, usuarioDto);

        Assert.NotNull(result);
        Assert.Equal("Role não encontrada", result.Message);
        Assert.True(result.ServerOn);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task UpdateUsuario_ShouldReturnConflict_WhenEmailAlreadyExists()
    {
        var usuarioId = Guid.NewGuid();
        var usuarioDto = new UsuarioDto { Nome = "Nome", Email = "existing@email.com", Password = "12345678" };
        var existingUsuario = new Usuario { Id = usuarioId, Nome = "Nome", Email = "existing@email.com", PasswordHash = "312mk312kosSmdiosp" };

        _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(usuarioId))
            .ReturnsAsync(existingUsuario);

        _usuarioRepositoryMock.Setup(repo => repo.GetByEmail(usuarioDto.Email))
            .ReturnsAsync(existingUsuario);

        var result = await _service.UpdateUsuario(usuarioId, usuarioDto);

        Assert.NotNull(result);
        Assert.True(result.ServerOn);
        Assert.Equal("Email já cadastrado", result.Message);
        Assert.Equal(409, result.StatusCode);
    }

    [Fact]
    public async Task UpdateUsuario_ShouldLogError_WhenExceptionOccurs()
    {
        var usuarioId = Guid.NewGuid();
        var usuarioDto = new UsuarioDto { Nome = "Nome", Email = "email@email.com", Password = "12345678" };
        var exception = new Exception("Database error");

        _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(usuarioId))
            .ThrowsAsync(exception);

        var result = await _service.UpdateUsuario(usuarioId, usuarioDto);

        Assert.NotNull(result);
        Assert.False(result.ServerOn);
        Assert.Equal("Erro inesperado: Database error", result.Message);
        Assert.Equal(500, result.StatusCode);
    }
}
