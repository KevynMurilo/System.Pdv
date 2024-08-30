using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.Services.Usuarios;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.Services.Usuarios;

public class GetAllUsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<ILogger<GetAllUsuarioService>> _loggerMock;
    private readonly GetAllUsuarioService _service;

    public GetAllUsuarioServiceTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _loggerMock = new Mock<ILogger<GetAllUsuarioService>>();
        _service = new GetAllUsuarioService(_usuarioRepositoryMock.Object, _loggerMock.Object);   
    }

    [Fact]
    public async Task GetAllUsuario_ReturnsUsuarios_WhenNoUsuariosExist()
    {
        _usuarioRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Usuario>());
        
        var result = await _service.GetAllUsuario();

        Assert.True(result.ServerOn);
        Assert.Equal("Nenhum usuário encontrado", result.Message);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task GetAllUsuario_ReturnsMesas_WhenMesasExist()
    {
        var usuarios = new List<Usuario>
        {
            new Usuario { Id = Guid.NewGuid(), Nome = "User1", Email = "Email1@example.com", PasswordHash = "123" },
            new Usuario { Id = Guid.NewGuid(), Nome = "User2", Email = "Email2@example.com", PasswordHash = "123" },
        };

        _usuarioRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(usuarios);

        var result = await _service.GetAllUsuario();

        Assert.True(result.ServerOn);
        Assert.Equal(usuarios, result.Result.ToList());
    }

    [Fact]
    public async Task GetAllUsuarios_LogsError_WhenExceptionIsThrown()
    {
        var exception = new Exception("Database failure");
        _usuarioRepositoryMock.Setup(repo => repo.GetAllAsync()).ThrowsAsync(exception);

        var result = await _service.GetAllUsuario();

        Assert.False(result.ServerOn);
        Assert.Equal("Erro inesperado: Database failure", result.Message);
        Assert.Equal(500, result.StatusCode);
    }
}
