using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.UseCase.Roles;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.UseCase.Roles;

public class UpdateRoleUseCaseTests
{
    private readonly Mock<IRoleRepository> _mockRoleRepository;
    private readonly Mock<ILogger<UpdateRoleUseCase>> _mockLogger;
    private readonly UpdateRoleUseCase _updateRoleUseCase;

    public UpdateRoleUseCaseTests()
    {
        _mockRoleRepository = new Mock<IRoleRepository>();
        _mockLogger = new Mock<ILogger<UpdateRoleUseCase>>();
        _updateRoleUseCase = new UpdateRoleUseCase(_mockRoleRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ExecuteAsync_RoleNotFound_ReturnsNotFoundResult()
    {
        var roleId = Guid.NewGuid();
        var roleDto = new RoleDto { Nome = "UpdatedRole", Descricao = "Updated description" };
        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(roleId)).ReturnsAsync((Role)null);

        var result = await _updateRoleUseCase.ExecuteAsync(roleId, roleDto);

        Assert.True(result.ReqSuccess);
        Assert.Equal("Role não encontrada", result.Message);
        Assert.Equal(404, result.StatusCode);
        _mockRoleRepository.Verify(repo => repo.GetByIdAsync(roleId), Times.Once);
        _mockRoleRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Role>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_AttemptToUpdateStandardRole_ReturnsBadRequest()
    {
        var roleId = Guid.NewGuid();
        var role = new Role { Nome = "ADMIN" };
        var roleDto = new RoleDto { Nome = "UpdatedRole", Descricao = "Updated description" };
        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(roleId)).ReturnsAsync(role);

        var result = await _updateRoleUseCase.ExecuteAsync(roleId, roleDto);

        Assert.True(result.ReqSuccess);
        Assert.Equal("Não é possível atualizar as roles padrão", result.Message);
        Assert.Equal(400, result.StatusCode);
        _mockRoleRepository.Verify(repo => repo.GetByIdAsync(roleId), Times.Once);
        _mockRoleRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Role>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_RoleNameAlreadyRegistered_ReturnsConflictResult()
    {
        var roleId = Guid.NewGuid();
        var existingRole = new Role { Nome = "ExistingRole" };
        var roleDto = new RoleDto { Nome = "ExistingRole", Descricao = "Updated description" };
        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(roleId)).ReturnsAsync(new Role { Nome = "SomeRole" });
        _mockRoleRepository.Setup(repo => repo.GetByNameAsync(roleDto.Nome)).ReturnsAsync(existingRole);

        var result = await _updateRoleUseCase.ExecuteAsync(roleId, roleDto);

        Assert.True(result.ReqSuccess);
        Assert.Equal("Role já registrada", result.Message);
        Assert.Equal(404, result.StatusCode);
        _mockRoleRepository.Verify(repo => repo.GetByIdAsync(roleId), Times.Once);
        _mockRoleRepository.Verify(repo => repo.GetByNameAsync(roleDto.Nome), Times.Once);
        _mockRoleRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Role>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_UpdateRoleSuccessfully_ReturnsSuccessResult()
    {
        var roleId = Guid.NewGuid();
        var existingRole = new Role { Nome = "ExistingRole" };
        var roleDto = new RoleDto { Nome = "UpdatedRole", Descricao = "Updated description" };
        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(roleId)).ReturnsAsync(existingRole);
        _mockRoleRepository.Setup(repo => repo.GetByNameAsync(roleDto.Nome)).ReturnsAsync((Role)null);
        _mockRoleRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Role>())).Returns(Task.CompletedTask);

        var result = await _updateRoleUseCase.ExecuteAsync(roleId, roleDto);

        Assert.True(result.ReqSuccess);
        Assert.Equal("Role atualizada com sucesso", result.Message);
        Assert.Equal(roleDto.Nome.ToUpper(), result.Result.Nome);
        Assert.Equal(roleDto.Descricao, result.Result.Descricao);
        _mockRoleRepository.Verify(repo => repo.GetByIdAsync(roleId), Times.Once);
        _mockRoleRepository.Verify(repo => repo.GetByNameAsync(roleDto.Nome), Times.Once);
        _mockRoleRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Role>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ExceptionThrown_ReturnsErrorResult()
    {
        var roleId = Guid.NewGuid();
        var roleDto = new RoleDto { Nome = "UpdatedRole", Descricao = "Updated description" };
        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(roleId)).ThrowsAsync(new Exception("Database error"));

        var result = await _updateRoleUseCase.ExecuteAsync(roleId, roleDto);

        Assert.False(result.ReqSuccess);
        Assert.StartsWith("Erro inesperado:", result.Message);
        Assert.Equal(500, result.StatusCode);
        _mockRoleRepository.Verify(repo => repo.GetByIdAsync(roleId), Times.Once);
        _mockRoleRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Role>()), Times.Never);
    }
}
