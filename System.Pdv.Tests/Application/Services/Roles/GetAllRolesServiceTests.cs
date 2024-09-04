using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Pdv.Application.Services.Roles;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Tests.Application.Services.Roles;
public class GetAllRolesServiceTests
{
    private readonly Mock<IRoleRepository> _roleRepositoryMock;
    private readonly Mock<ILogger<GetAllRolesService>> _loggerMock;
    private readonly GetAllRolesService _getAllRolesService;

    public GetAllRolesServiceTests()
    {
        _roleRepositoryMock = new Mock<IRoleRepository>();
        _loggerMock = new Mock<ILogger<GetAllRolesService>>();
        _getAllRolesService = new GetAllRolesService(_roleRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllRoles_ShouldReturnRoles_WhenRolesExist()
    {
        var roles = new List<Role>
        {
            new Role { Id = Guid.NewGuid(), Nome = "Admin" },
            new Role { Id = Guid.NewGuid(), Nome = "Garcom" }
        };

        _roleRepositoryMock.Setup(repo => repo.GetAllRolesAsync()).ReturnsAsync(roles);

        var result = await _getAllRolesService.ExecuteAsync();

        Assert.NotNull(result.Result);
        Assert.Equal(roles.Count, result.Result.Count());
        Assert.Equal("Admin", result.Result.First().Nome);
        Assert.Null(result.Message);
        Assert.Equal(200, result.StatusCode);
        _roleRepositoryMock.Verify(repo => repo.GetAllRolesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllRoles_ShouldReturnNotFound_WhenNoRolesExist()
    {
        var roles = new List<Role>();

        _roleRepositoryMock.Setup(repo => repo.GetAllRolesAsync()).ReturnsAsync(roles);

        var result = await _getAllRolesService.ExecuteAsync();

        Assert.Null(result.Result);
        Assert.Equal("Nenhuma role encontrada", result.Message);
        Assert.Equal(404, result.StatusCode);
        _roleRepositoryMock.Verify(repo => repo.GetAllRolesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllRoles_ShouldLogError_WhenExceptionOccurs()
    {
        _roleRepositoryMock.Setup(repo => repo.GetAllRolesAsync()).ThrowsAsync(new Exception("Database error"));

        var result = await _getAllRolesService.ExecuteAsync();

        Assert.Null(result.Result);
        Assert.Equal("Erro inesperado: Database error", result.Message);
        Assert.Equal(500, result.StatusCode);
        _roleRepositoryMock.Verify(repo => repo.GetAllRolesAsync(), Times.Once);
    }
}
