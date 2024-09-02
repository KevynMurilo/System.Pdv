using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Services.Usuarios;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.Services.Usuarios
{
    public class CreateUsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<ILogger<CreateUsuarioService>> _loggerMock;
        private readonly CreateUsuarioService _createUsuarioService;

        public CreateUsuarioServiceTests()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _loggerMock = new Mock<ILogger<CreateUsuarioService>>();
            _createUsuarioService = new CreateUsuarioService(_usuarioRepositoryMock.Object, _roleRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateUsuario_ShouldReturnOperationResultWithUsuario_WhenSuccessful()
        {
            var roleId = Guid.NewGuid();
            var usuarioDto = new UsuarioDto { Nome = "Teste", RoleId = roleId, Email = "teste@example.com", Password = "senha123" };
            var existingRole = new Role { Id = roleId, Nome = "Usuario", Descricao = "descricao" };

            _usuarioRepositoryMock.Setup(repo => repo.GetByEmail(usuarioDto.Email))
                .ReturnsAsync((Usuario)null);

            _roleRepositoryMock.Setup(repo => repo.GetByIdAsync(roleId))
            .ReturnsAsync(existingRole);

            _usuarioRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Usuario>()))
                .Returns(Task.CompletedTask);

            var result = await _createUsuarioService.CreateUsuario(usuarioDto);

            Assert.NotNull(result);
            Assert.True(result.ServerOn);
            Assert.Equal(usuarioDto.Nome, result.Result.Nome);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task CreateUsuario_ShouldReturn_NotFoundRole()
        {
            var roleId = Guid.NewGuid();
            var usuarioDto = new UsuarioDto { Nome = "Teste", RoleId = roleId, Email = "teste@example.com", Password = "senha123" };

            _usuarioRepositoryMock.Setup(repo => repo.GetByEmail(usuarioDto.Email))
                .ReturnsAsync((Usuario)null);

            _roleRepositoryMock.Setup(repo => repo.GetByIdAsync(roleId))
            .ReturnsAsync((Role)null);

            _usuarioRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Usuario>()))
                .Returns(Task.CompletedTask);

            var result = await _createUsuarioService.CreateUsuario(usuarioDto);

            Assert.NotNull(result);
            Assert.True(result.ServerOn);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Role não encontrada", result.Message);
        }

        [Fact]
        public async Task CreateUsuario_ShouldReturnConflict_WhenEmailAlreadyExists()
        {
            var usuarioDto = new UsuarioDto { Nome = "Kevyn", Email = "kevyn@email.com", Password = "12345678" };

            var existingUsuario = new Usuario { Nome = "Kevyn", Email = "kevyn@email.com", PasswordHash = "hashedpassword" };

            _usuarioRepositoryMock.Setup(repo => repo.GetByEmail(usuarioDto.Email))
                .ReturnsAsync(existingUsuario);

            var result = await _createUsuarioService.CreateUsuario(usuarioDto);

            Assert.True(result.ServerOn);
            Assert.Equal(409, result.StatusCode);
            Assert.Equal("Email já cadastrado", result.Message);
        }

        [Fact]
        public async Task CreateUsuario_ShouldReturnError_WhenExceptionIsThrown()
        {
            var roleId = Guid.NewGuid();
            var existingRole = new Role { Id = roleId, Nome = "Usuario", Descricao = "descricao" };
            var usuarioDto = new UsuarioDto { Nome = "João", RoleId = roleId, Email = "joao@example.com", Password = "password123" };

            _usuarioRepositoryMock.Setup(repo => repo.GetByEmail(usuarioDto.Email))
                .ReturnsAsync((Usuario)null);

            _roleRepositoryMock.Setup(repo => repo.GetByIdAsync(roleId))
                .ReturnsAsync(existingRole);

            _usuarioRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Usuario>()))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _createUsuarioService.CreateUsuario(usuarioDto);

            Assert.False(result.ServerOn);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal("Erro inesperado: Database error", result.Message);
        }
    }
}
