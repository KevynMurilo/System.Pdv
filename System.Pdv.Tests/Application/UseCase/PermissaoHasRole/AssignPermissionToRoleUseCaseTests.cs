using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using System.Pdv.Application.UseCase.PermissaoHasRole;

namespace System.Pdv.Application.Tests.UseCase.PermissaoHasRole;

public class AssignPermissionToRoleUseCaseTests
{
    private readonly Mock<IRoleRepository> _mockRoleRepository;
    private readonly Mock<IPermissaoRepository> _mockPermissaoRepository;
    private readonly Mock<ILogger<AssignPermissionToRoleUseCase>> _mockLogger;
    private readonly AssignPermissionToRoleUseCase _useCase;

    public AssignPermissionToRoleUseCaseTests()
    {
        _mockRoleRepository = new Mock<IRoleRepository>();
        _mockPermissaoRepository = new Mock<IPermissaoRepository>();
        _mockLogger = new Mock<ILogger<AssignPermissionToRoleUseCase>>();
        _useCase = new AssignPermissionToRoleUseCase(
            _mockRoleRepository.Object,
            _mockPermissaoRepository.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnSuccess_WhenRoleAndPermissionExistAndNotAssigned()
    {
        var roleId = Guid.NewGuid();
        var permissaoId = Guid.NewGuid();
        var role = new Role { Id = roleId, Permissoes = new List<Permissao>() };
        var permissao = new Permissao { Id = permissaoId };

        _mockRoleRepository.Setup(r => r.GetByIdAsync(roleId)).ReturnsAsync(role);
        _mockPermissaoRepository.Setup(p => p.GetByIdAsync(permissaoId)).ReturnsAsync(permissao);
        _mockRoleRepository.Setup(r => r.UpdateAsync(It.IsAny<Role>())).Returns(Task.CompletedTask);

        var result = await _useCase.ExecuteAsync(roleId, permissaoId);

        Assert.True(result.Result);
        Assert.Single(role.Permissoes);
        Assert.Contains(permissao, role.Permissoes);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenRoleDoesNotExist()
    {
        var roleId = Guid.NewGuid();
        var permissaoId = Guid.NewGuid();

        _mockRoleRepository.Setup(r => r.GetByIdAsync(roleId)).ReturnsAsync((Role)null);
        _mockPermissaoRepository.Setup(p => p.GetByIdAsync(permissaoId)).ReturnsAsync(new Permissao());

        var result = await _useCase.ExecuteAsync(roleId, permissaoId);

        Assert.False(result.Result);
        Assert.Equal("Role não encontrada", result.Message);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNotFound_WhenPermissionDoesNotExist()
    {
        var roleId = Guid.NewGuid();
        var permissaoId = Guid.NewGuid();
        var role = new Role { Id = roleId, Permissoes = new List<Permissao>() };

        _mockRoleRepository.Setup(r => r.GetByIdAsync(roleId)).ReturnsAsync(role);
        _mockPermissaoRepository.Setup(p => p.GetByIdAsync(permissaoId)).ReturnsAsync((Permissao)null);

        var result = await _useCase.ExecuteAsync(roleId, permissaoId);

        Assert.False(result.Result);
        Assert.Equal("Permissão não encontrada", result.Message);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnError_WhenExceptionOccurs()
    {
        var roleId = Guid.NewGuid();
        var permissaoId = Guid.NewGuid();

        _mockRoleRepository.Setup(r => r.GetByIdAsync(roleId)).ThrowsAsync(new Exception("Database error"));
        _mockPermissaoRepository.Setup(p => p.GetByIdAsync(permissaoId)).ThrowsAsync(new Exception("Database error"));

        var result = await _useCase.ExecuteAsync(roleId, permissaoId);

        Assert.False(result.Result);
        Assert.StartsWith("Erro inesperado:", result.Message);
        Assert.Equal(500, result.StatusCode);
    }
}
