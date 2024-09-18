using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Pdv.Application.UseCase.Roles;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.UseCase.Roles;

public class CreateRoleUseCaseTests
{
    private readonly Mock<IRoleRepository> _mockRoleRepository;
    private readonly Mock<ILogger<CreateRoleUseCase>> _mockLogger;
    private readonly CreateRoleUseCase _createRoleUseCase;

    public CreateRoleUseCaseTests()
    {
        _mockRoleRepository = new Mock<IRoleRepository>();
        _mockLogger = new Mock<ILogger<CreateRoleUseCase>>();
        _createRoleUseCase = new CreateRoleUseCase(_mockRoleRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ExecuteAsync_RoleAlreadyExists_ReturnsConflictResult()
    {
        var roleDto = new RoleDto { Nome = "EXISTINGROLE", Descricao = "Role already exists" };
        _mockRoleRepository.Setup(repo => repo.GetByNameAsync(roleDto.Nome)).ReturnsAsync(new Role { Nome = "EXISTINGROLE" });

        var result = await _createRoleUseCase.ExecuteAsync(roleDto);

        Assert.True(result.ServerOn);
        Assert.Equal("Role já cadastrada", result.Message);
        Assert.Equal(409, result.StatusCode);
        _mockRoleRepository.Verify(repo => repo.GetByNameAsync(roleDto.Nome), Times.Once);
        _mockRoleRepository.Verify(repo => repo.AddAsync(It.IsAny<Role>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_CreateRoleSuccessfully_ReturnsSuccessResult()
    {
        var roleDto = new RoleDto { Nome = "NEWROLE", Descricao = "Newly created role" };
        _mockRoleRepository.Setup(repo => repo.GetByNameAsync(roleDto.Nome)).ReturnsAsync((Role)null);
        _mockRoleRepository.Setup(repo => repo.AddAsync(It.IsAny<Role>())).Returns(Task.CompletedTask);

        var result = await _createRoleUseCase.ExecuteAsync(roleDto);

        Assert.True(result.ServerOn);
        Assert.Null(result.Message);
        Assert.NotNull(result.Result);
        Assert.Equal(roleDto.Nome.ToUpper(), result.Result.Nome);
        _mockRoleRepository.Verify(repo => repo.GetByNameAsync(roleDto.Nome), Times.Once);
        _mockRoleRepository.Verify(repo => repo.AddAsync(It.IsAny<Role>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ExceptionThrown_ReturnsErrorResult()
    {
        var roleDto = new RoleDto { Nome = "ROLETHATFAILS", Descricao = "This will fail" };
        _mockRoleRepository.Setup(repo => repo.GetByNameAsync(roleDto.Nome)).ThrowsAsync(new Exception("Database error"));

        var result = await _createRoleUseCase.ExecuteAsync(roleDto);

        Assert.False(result.ServerOn);
        Assert.StartsWith("Erro inesperado:", result.Message);
        Assert.Equal(500, result.StatusCode);
        _mockRoleRepository.Verify(repo => repo.GetByNameAsync(roleDto.Nome), Times.Once);
        _mockRoleRepository.Verify(repo => repo.AddAsync(It.IsAny<Role>()), Times.Never);
    }
}
