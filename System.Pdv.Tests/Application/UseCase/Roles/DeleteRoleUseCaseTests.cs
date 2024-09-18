using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.UseCase.Roles;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.UseCase.Roles;

public class DeleteRoleUseCaseTests
{
    private readonly Mock<IRoleRepository> _mockRoleRepository;
    private readonly Mock<ILogger<DeleteRoleUseCase>> _mockLogger;
    private readonly DeleteRoleUseCase _deleteRoleUseCase;

    public DeleteRoleUseCaseTests()
    {
        _mockRoleRepository = new Mock<IRoleRepository>();
        _mockLogger = new Mock<ILogger<DeleteRoleUseCase>>();
        _deleteRoleUseCase = new DeleteRoleUseCase(_mockRoleRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ExecuteAsync_RoleNotFound_ReturnsNotFoundResult()
    {
        var roleId = Guid.NewGuid();
        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(roleId)).ReturnsAsync((Role)null);

        var result = await _deleteRoleUseCase.ExecuteAsync(roleId);

        Assert.True(result.ServerOn);
        Assert.Equal("Role não encontrada", result.Message);
        Assert.Equal(404, result.StatusCode);
        _mockRoleRepository.Verify(repo => repo.GetByIdAsync(roleId), Times.Once);
        _mockRoleRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Role>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_AttemptToDeleteStandardRole_ReturnsBadRequest()
    {
        var roleId = Guid.NewGuid();
        var role = new Role { Nome = "ADMIN" };
        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(roleId)).ReturnsAsync(role);

        var result = await _deleteRoleUseCase.ExecuteAsync(roleId);

        Assert.True(result.ServerOn);
        Assert.Equal("Não é possível excluir as roles padrão", result.Message);
        Assert.Equal(400, result.StatusCode);
        _mockRoleRepository.Verify(repo => repo.GetByIdAsync(roleId), Times.Once);
        _mockRoleRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Role>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_DeleteRoleSuccessfully_ReturnsSuccessResult()
    {
        var roleId = Guid.NewGuid();
        var role = new Role { Nome = "UserRole" };
        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(roleId)).ReturnsAsync(role);
        _mockRoleRepository.Setup(repo => repo.DeleteAsync(role)).Returns(Task.CompletedTask);

        var result = await _deleteRoleUseCase.ExecuteAsync(roleId);

        Assert.True(result.ServerOn);
        Assert.Equal($"Role {role.Nome} deletado com sucesso", result.Message);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(role, result.Result);
        _mockRoleRepository.Verify(repo => repo.GetByIdAsync(roleId), Times.Once);
        _mockRoleRepository.Verify(repo => repo.DeleteAsync(role), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ExceptionThrown_ReturnsErrorResult()
    {
        var roleId = Guid.NewGuid();
        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(roleId)).ThrowsAsync(new Exception("Database error"));

        var result = await _deleteRoleUseCase.ExecuteAsync(roleId);

        Assert.False(result.ServerOn);
        Assert.StartsWith("Erro inesperado:", result.Message);
        Assert.Equal(500, result.StatusCode);
        _mockRoleRepository.Verify(repo => repo.GetByIdAsync(roleId), Times.Once);
        _mockRoleRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Role>()), Times.Never);
    }
}
