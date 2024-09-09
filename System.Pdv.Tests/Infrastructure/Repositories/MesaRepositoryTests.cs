using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Pdv.Core.Entities;
using System.Pdv.Infrastructure.Data;
using System.Pdv.Infrastructure.Repositories;

namespace System.Pdv.Tests.Infrastructure.Repositories;

public class MesaRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly MesaRepository _repository;

    public MesaRepositoryTests()
    {
        var uniqueDbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: uniqueDbName)
            .Options;

        _context = new AppDbContext(options);
        _repository = new MesaRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddMesa()
    {
        var mesa = new Mesa
        {
            Id = Guid.NewGuid(),
            Numero = 1
        };

        var addedMesa = await _repository.AddAsync(mesa);

        var retrievedMesa = await _repository.GetByIdAsync(mesa.Id);
        Assert.NotNull(retrievedMesa);
        Assert.Equal(mesa.Numero, retrievedMesa?.Numero);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateMesa()
    {
        var mesa = new Mesa
        {
            Id = Guid.NewGuid(),
            Numero = 1
        };

        await _repository.AddAsync(mesa);

        mesa.Numero = 2;
        await _repository.UpdateAsync(mesa);

        var updatedMesa = await _repository.GetByIdAsync(mesa.Id);
        Assert.Equal(2, updatedMesa?.Numero);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllMesas()
    {
        _context.Mesas.RemoveRange(_context.Mesas);
        var mesa1 = new Mesa
        {
            Id = Guid.NewGuid(),
            Numero = 1
        };
        var mesa2 = new Mesa
        {
            Id = Guid.NewGuid(),
            Numero = 2
        };

        await _repository.AddAsync(mesa1);
        await _repository.AddAsync(mesa2);

        var results = await _repository.GetAllAsync();
        Assert.Equal(2, results.Count());
    }

    [Fact]
    public async Task GetByNumberAsync_ShouldReturnMesa()
    {
        var mesa = new Mesa
        {
            Id = Guid.NewGuid(),
            Numero = 3
        };

        await _repository.AddAsync(mesa);

        var result = await _repository.GetByNumberAsync(3);
        Assert.NotNull(result);
        Assert.Equal(mesa.Id, result?.Id);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveMesa()
    {
        var mesa = new Mesa
        {
            Id = Guid.NewGuid(),
            Numero = 4
        };

        await _repository.AddAsync(mesa);
        await _repository.DeleteAsync(mesa);

        var deletedMesa = await _repository.GetByIdAsync(mesa.Id);
        Assert.Null(deletedMesa);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
