using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Pdv.Core.Entities;
using System.Pdv.Infrastructure.Data;
using System.Pdv.Infrastructure.Repositories;

namespace System.Pdv.Infrastructure.Tests.Repositories;
public class PermissaoRepositoryTests
{
    private readonly AppDbContext _context;
    private readonly PermissaoRepository _repository;

    public PermissaoRepositoryTests()
    {
        var uniqueDbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: uniqueDbName)
            .Options;

        _context = new AppDbContext(options);
        _repository = new PermissaoRepository(_context);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnPermissao_WhenPermissaoExists()
    {
        var permissao = new Permissao
        {
            Id = Guid.NewGuid(),
            Recurso = "Resource",
            Acao = "Action"
        };

        await _context.Permissoes.AddAsync(permissao);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(permissao.Id);

        Assert.NotNull(result);
        Assert.Equal(permissao.Id, result?.Id);
        Assert.Equal(permissao.Recurso, result?.Recurso);
        Assert.Equal(permissao.Acao, result?.Acao);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenPermissaoDoesNotExist()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPermissoes()
    {
        var permissao1 = new Permissao { Id = Guid.NewGuid(), Recurso = "Resource1", Acao = "Action1" };
        var permissao2 = new Permissao { Id = Guid.NewGuid(), Recurso = "Resource2", Acao = "Action2" };

        await _context.Permissoes.AddRangeAsync(permissao1, permissao2);
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task AddAsync_ShouldAddPermissao()
    {
        var permissao = new Permissao
        {
            Id = Guid.NewGuid(),
            Recurso = "Resource",
            Acao = "Action"
        };

        await _repository.AddAsync(permissao);

        var result = await _context.Permissoes.FindAsync(permissao.Id);

        Assert.NotNull(result);
        Assert.Equal(permissao.Id, result?.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdatePermissao()
    {
        var permissao = new Permissao
        {
            Id = Guid.NewGuid(),
            Recurso = "OldResource",
            Acao = "OldAction"
        };

        await _context.Permissoes.AddAsync(permissao);
        await _context.SaveChangesAsync();

        permissao.Recurso = "NewResource";
        permissao.Acao = "NewAction";

        await _repository.UpdateAsync(permissao);

        var result = await _context.Permissoes.FindAsync(permissao.Id);

        Assert.NotNull(result);
        Assert.Equal("NewResource", result?.Recurso);
        Assert.Equal("NewAction", result?.Acao);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemovePermissao()
    {
        var permissao = new Permissao
        {
            Id = Guid.NewGuid(),
            Recurso = "Resource",
            Acao = "Action"
        };

        await _context.Permissoes.AddAsync(permissao);
        await _context.SaveChangesAsync();

        await _repository.DeleteAsync(permissao);

        var result = await _context.Permissoes.FindAsync(permissao.Id);

        Assert.Null(result);
    }
}
