using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.UseCase.Auth;
using System.Pdv.Core.Entities;
using System.Pdv.Application.Interfaces.Usuarios;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.UseCase.Auth;

public class AuthServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IJwtTokenGeneratorUsuario> _tokenGeneratorMock;
    private readonly Mock<ILogger<AuthUseCase>> _loggerMock;
    private readonly AuthUseCase _authService;

    public AuthServiceTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _tokenGeneratorMock = new Mock<IJwtTokenGeneratorUsuario>();
        _loggerMock = new Mock<ILogger<AuthUseCase>>();
        _authService = new AuthUseCase(_usuarioRepositoryMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task AuthenticateAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        var loginDto = new LoginDto { Email = "user@test.com", Password = "Password123" };
        var usuario = new Usuario { Email = loginDto.Email, PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginDto.Password) };
        var token = "jwt-token";

        _usuarioRepositoryMock.Setup(repo => repo.GetByEmail(loginDto.Email)).ReturnsAsync(usuario);
        _tokenGeneratorMock.Setup(generator => generator.GenerateToken(usuario)).Returns(token);

        var result = await _authService.ExecuteAsync(loginDto);

        Assert.NotNull(result.Result);
        Assert.Null(result.Message);
        Assert.Equal(200, result.StatusCode);

        _usuarioRepositoryMock.Verify(repo => repo.GetByEmail(It.IsAny<string>()), Times.Once);
        _tokenGeneratorMock.Verify(generator => generator.GenerateToken(It.IsAny<Usuario>()), Times.Once);
    }


    [Fact]
    public async Task AuthenticateAsync_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        var loginDto = new LoginDto { Email = "user@test.com", Password = "WrongPassword" };
        var usuario = new Usuario { Email = loginDto.Email, PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword") };

        _usuarioRepositoryMock.Setup(repo => repo.GetByEmail(loginDto.Email)).ReturnsAsync(usuario);

        var result = await _authService.ExecuteAsync(loginDto);

        Assert.Null(result.Result);
        Assert.Equal("Credenciais inválidas", result.Message);
        Assert.Equal(401, result.StatusCode);
        _usuarioRepositoryMock.Verify(repo => repo.GetByEmail(It.IsAny<string>()), Times.Once);
        _tokenGeneratorMock.Verify(generator => generator.GenerateToken(It.IsAny<Usuario>()), Times.Never);
    }

    [Fact]
    public async Task AuthenticateAsync_ShouldReturnUnauthorized_WhenUserNotFound()
    {
        var loginDto = new LoginDto { Email = "user@test.com", Password = "Password123" };

        _usuarioRepositoryMock.Setup(repo => repo.GetByEmail(loginDto.Email)).ReturnsAsync((Usuario)null);

        var result = await _authService.ExecuteAsync(loginDto);

        Assert.Null(result.Result);
        Assert.Equal("Credenciais inválidas", result.Message);
        Assert.Equal(401, result.StatusCode);
        _usuarioRepositoryMock.Verify(repo => repo.GetByEmail(It.IsAny<string>()), Times.Once);
        _tokenGeneratorMock.Verify(generator => generator.GenerateToken(It.IsAny<Usuario>()), Times.Never);
    }

    [Fact]
    public async Task AuthenticateAsync_ShouldLogError_WhenExceptionOccurs()
    {
        var loginDto = new LoginDto { Email = "user@test.com", Password = "Password123" };

        _usuarioRepositoryMock.Setup(repo => repo.GetByEmail(loginDto.Email)).ThrowsAsync(new Exception("Database error"));

        var result = await _authService.ExecuteAsync(loginDto);

        Assert.Null(result.Result);
        Assert.Equal("Erro inesperado: Database error", result.Message);
        Assert.Equal(500, result.StatusCode);
        _usuarioRepositoryMock.Verify(repo => repo.GetByEmail(It.IsAny<string>()), Times.Once);
        _tokenGeneratorMock.Verify(generator => generator.GenerateToken(It.IsAny<Usuario>()), Times.Never);
    }
}
