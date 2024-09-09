using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Pdv.Core.Entities;
using System.Pdv.Infrastructure.Data;
using System.Pdv.Infrastructure.Repositories;

namespace System.Pdv.Tests.Infrastructure.Repositories;

public class UsuarioRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly UsuarioRepository _repository;

    public UsuarioRepositoryTests()
    {
        var uniqueDbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: uniqueDbName)
            .Options;

        _context = new AppDbContext(options);
        _repository = new UsuarioRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddUsuario()
    {
        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Role = new Role { Id = Guid.NewGuid(), Nome = "Initial Role" },
            Nome = "Test Usuario",
            Email = "test@example.com",
        };

        await _repository.AddAsync(usuario);

        var addedUsuario = await _repository.GetByIdAsync(usuario.Id);
        Assert.NotNull(addedUsuario);
        Assert.Equal(usuario.Id, addedUsuario.Id);
        Assert.Equal("Test Usuario", addedUsuario.Nome);
        Assert.Equal("test@example.com", addedUsuario.Email);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUsuario()
    {
        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = "Initial Name",
            Email = "initial@example.com",
            Role = new Role { Id = Guid.NewGuid(), Nome = "Initial Role" }
        };

        await _repository.AddAsync(usuario);

        usuario.Nome = "Updated Name";
        usuario.Email = "updated@example.com";
        usuario.Role.Nome = "Updated Role";
        await _repository.UpdateAsync(usuario);

        var updatedUsuario = await _repository.GetByIdAsync(usuario.Id);
        Assert.NotNull(updatedUsuario);
        Assert.Equal("Updated Name", updatedUsuario?.Nome);
        Assert.Equal("updated@example.com", updatedUsuario?.Email);
        Assert.Equal("Updated Role", updatedUsuario?.Role.Nome);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsuarios()
    {
        var usuario1 = new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = "Usuario 1",
            Email = "usuario1@example.com",
            Role = new Role { Id = Guid.NewGuid(), Nome = "Role 1" }
        };
        var usuario2 = new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = "Usuario 2",
            Email = "usuario2@example.com",
            Role = new Role { Id = Guid.NewGuid(), Nome = "Role 2" }
        };

        await _repository.AddAsync(usuario1);
        await _repository.AddAsync(usuario2);

        var results = await _repository.GetAllAsync(1, 10);
        Assert.Equal(2, results.Count());
        Assert.Contains(results, u => u.Nome == "Usuario 1");
        Assert.Contains(results, u => u.Nome == "Usuario 2");
    }

    [Fact]
    public async Task GetByEmail_ShouldReturnUsuarioByEmail()
    {
        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = "Test Usuario",
            Email = "test@example.com",
            Role = new Role { Id = Guid.NewGuid(), Nome = "Test Role" }
        };

        await _repository.AddAsync(usuario);

        var result = await _repository.GetByEmail("test@example.com");
        Assert.NotNull(result);
        Assert.Equal("Test Usuario", result?.Nome);
        Assert.Equal("Test Role", result?.Role.Nome);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveUsuario()
    {
        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = "Test Usuario",
            Email = "test@example.com",
            Role = new Role { Id = Guid.NewGuid(), Nome = "Test Role" }
        };

        await _repository.AddAsync(usuario);

        await _repository.DeleteAsync(usuario);

        var deletedUsuario = await _repository.GetByIdAsync(usuario.Id);
        Assert.Null(deletedUsuario);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
