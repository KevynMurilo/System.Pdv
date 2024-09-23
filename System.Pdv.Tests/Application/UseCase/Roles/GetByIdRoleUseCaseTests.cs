using Microsoft.Extensions.Logging;
using Moq;
using System.Pdv.Application.UseCase.Roles;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;
using Xunit;

namespace System.Pdv.Tests.Application.UseCase.Roles;

public class GetByIdRoleUseCaseTests
{
    private readonly Mock<IRoleRepository> _mockRoleRepository;
    private readonly Mock<ILogger<GetByIdRoleUseCase>> _mockLogger;
    private readonly GetByIdRoleUseCase _getByIdRoleUseCase;

    public GetByIdRoleUseCaseTests()
    {
        _mockRoleRepository = new Mock<IRoleRepository>();
        _mockLogger = new Mock<ILogger<GetByIdRoleUseCase>>();
        _getByIdRoleUseCase = new GetByIdRoleUseCase(_mockRoleRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ExecuteAsync_RoleNotFound_ReturnsNotFoundResult()
    {
        var roleId = Guid.NewGuid();
        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(roleId)).ReturnsAsync((Role)null);

        var result = await _getByIdRoleUseCase.ExecuteAsync(roleId);

        Assert.True(result.ReqSuccess);
        Assert.Equal("Role não encontrada", result.Message);
        Assert.Equal(404, result.StatusCode);
        _mockRoleRepository.Verify(repo => repo.GetByIdAsync(roleId), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_RoleFound_ReturnsRole()
    {
        var roleId = Guid.NewGuid();
        var role = new Role { Nome = "TestRole" };
        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(roleId)).ReturnsAsync(role);

        var result = await _getByIdRoleUseCase.ExecuteAsync(roleId);

        Assert.True(result.ReqSuccess);
        Assert.Equal(role, result.Result);
        Assert.Null(result.Message);
        Assert.Equal(200, result.StatusCode);
        _mockRoleRepository.Verify(repo => repo.GetByIdAsync(roleId), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ExceptionThrown_ReturnsServerError()
    {
        var roleId = Guid.NewGuid();
        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(roleId)).ThrowsAsync(new Exception("Database error"));

        var result = await _getByIdRoleUseCase.ExecuteAsync(roleId);

        Assert.False(result.ReqSuccess);
        Assert.Equal("Erro inesperado: Database error", result.Message);
        Assert.Equal(500, result.StatusCode);
    }
}
