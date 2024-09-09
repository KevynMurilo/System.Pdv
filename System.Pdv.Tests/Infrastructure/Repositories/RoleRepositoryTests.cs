using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Pdv.Core.Entities;
using System.Pdv.Infrastructure.Data;
using System.Pdv.Infrastructure.Repositories;

namespace System.Pdv.Tests.Infrastructure.Repositories;

public class RoleRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly RoleRepository _repository;

    public RoleRepositoryTests()
    {
        var uniqueDbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: uniqueDbName)
            .Options;

        _context = new AppDbContext(options);
        _repository = new RoleRepository(_context);
    }

    [Fact]
    public async Task GetAllRolesAsync_ShouldReturnAllRoles()
    {
        _context.Roles.RemoveRange(_context.Roles);
        await _context.SaveChangesAsync();

        var role1 = new Role
        {
            Id = Guid.NewGuid(),
            Nome = "Admin"
        };
        var role2 = new Role
        {
            Id = Guid.NewGuid(),
            Nome = "User"
        };

        _context.Roles.Add(role1);
        _context.Roles.Add(role2);
        await _context.SaveChangesAsync();

        var results = await _repository.GetAllRolesAsync();
        Assert.NotNull(results);
        Assert.Equal(2, results.Count());
    }


    [Fact]
    public async Task GetByIdAsync_ShouldReturnRole()
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Nome = "Manager"
        };

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(role.Id);
        Assert.NotNull(result);
        Assert.Equal(role.Nome, result?.Nome);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNullForNonExistentRole()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid());
        Assert.Null(result);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
