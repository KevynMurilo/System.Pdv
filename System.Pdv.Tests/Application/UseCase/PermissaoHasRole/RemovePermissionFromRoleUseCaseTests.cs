using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using System.Pdv.Application.UseCase.PermissaoHasRole;
using System.Pdv.Application.DTOs;

namespace System.Pdv.Application.Tests.UseCase.PermissaoHasRole;

public class RemovePermissionFromRoleUseCaseTests
{
    private readonly Mock<IRoleRepository> _mockRoleRepository;
    private readonly Mock<IPermissaoRepository> _mockPermissaoRepository;
    private readonly Mock<ILogger<RemovePermissionFromRoleUseCase>> _mockLogger;
    private readonly RemovePermissionFromRoleUseCase _useCase;

    public RemovePermissionFromRoleUseCaseTests()
    {
        _mockRoleRepository = new Mock<IRoleRepository>();
        _mockPermissaoRepository = new Mock<IPermissaoRepository>();
        _mockLogger = new Mock<ILogger<RemovePermissionFromRoleUseCase>>();
        _useCase = new RemovePermissionFromRoleUseCase(
            _mockRoleRepository.Object,
            _mockPermissaoRepository.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnSuccess_WhenRoleAndPermissionExistAndAssigned()
    {
        var roleId = Guid.NewGuid();
        var permissaoId = Guid.NewGuid();
        var permissao = new Permissao { Id = permissaoId };
        var role = new Role { Id = roleId, Permissoes = new List<Permissao> { permissao } };
        var permissionDto = new PermissionHasRoleDto
        {
            PermissaoIds = new List<Guid> { permissaoId },
            RoleIds = new List<Guid> { roleId }
        };

        _mockRoleRepository.Setup(r => r.GetByIdAsync(roleId)).ReturnsAsync(role);
        _mockPermissaoRepository.Setup(p => p.GetByIdAsync(permissaoId)).ReturnsAsync(permissao);
        _mockRoleRepository.Setup(r => r.UpdateAsync(It.IsAny<Role>())).Returns(Task.CompletedTask);

        var result = await _useCase.ExecuteAsync(permissionDto);

        Assert.True(result.Result);
        Assert.Empty(role.Permissoes); // Verifica que a permissão foi removida
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenRoleDoesNotExist()
    {
        var roleId = Guid.NewGuid();
        var permissaoId = Guid.NewGuid();
        var permissionDto = new PermissionHasRoleDto
        {
            PermissaoIds = new List<Guid> { permissaoId },
            RoleIds = new List<Guid> { roleId }
        };

        _mockRoleRepository.Setup(r => r.GetByIdAsync(roleId)).ReturnsAsync((Role)null);
        _mockPermissaoRepository.Setup(p => p.GetByIdAsync(permissaoId)).ReturnsAsync(new Permissao());

        var result = await _useCase.ExecuteAsync(permissionDto);

        Assert.False(result.Result);
        Assert.Equal("Role com ID " + roleId + " não encontrada", result.Message);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenPermissionDoesNotExist()
    {
        var roleId = Guid.NewGuid();
        var permissaoId = Guid.NewGuid();
        var role = new Role { Id = roleId, Permissoes = new List<Permissao>() };
        var permissionDto = new PermissionHasRoleDto
        {
            PermissaoIds = new List<Guid> { permissaoId },
            RoleIds = new List<Guid> { roleId }
        };

        _mockRoleRepository.Setup(r => r.GetByIdAsync(roleId)).ReturnsAsync(role);
        _mockPermissaoRepository.Setup(p => p.GetByIdAsync(permissaoId)).ReturnsAsync((Permissao)null);

        var result = await _useCase.ExecuteAsync(permissionDto);

        Assert.False(result.Result);
        Assert.Equal("Permissão com ID " + permissaoId + " não encontrada", result.Message);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnSuccess_WhenPermissionIsNotAssigned()
    {
        var roleId = Guid.NewGuid();
        var permissaoId = Guid.NewGuid();
        var role = new Role { Id = roleId, Permissoes = new List<Permissao>() };
        var permissao = new Permissao { Id = permissaoId };
        var permissionDto = new PermissionHasRoleDto
        {
            PermissaoIds = new List<Guid> { permissaoId },
            RoleIds = new List<Guid> { roleId }
        };

        _mockRoleRepository.Setup(r => r.GetByIdAsync(roleId)).ReturnsAsync(role);
        _mockPermissaoRepository.Setup(p => p.GetByIdAsync(permissaoId)).ReturnsAsync(permissao);

        var result = await _useCase.ExecuteAsync(permissionDto);

        Assert.True(result.Result);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnError_WhenExceptionOccurs()
    {
        var roleId = Guid.NewGuid();
        var permissaoId = Guid.NewGuid();
        var permissionDto = new PermissionHasRoleDto
        {
            PermissaoIds = new List<Guid> { permissaoId },
            RoleIds = new List<Guid> { roleId }
        };

        _mockRoleRepository.Setup(r => r.GetByIdAsync(roleId)).ThrowsAsync(new Exception("Database error"));
        _mockPermissaoRepository.Setup(p => p.GetByIdAsync(permissaoId)).ThrowsAsync(new Exception("Database error"));

        var result = await _useCase.ExecuteAsync(permissionDto);

        Assert.False(result.Result);
        Assert.StartsWith("Erro inesperado:", result.Message);
        Assert.Equal(500, result.StatusCode);
    }
}
