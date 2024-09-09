using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Pdv.Core.Entities;
using System.Pdv.Infrastructure.Data;
using System.Pdv.Infrastructure.Repositories;

namespace System.Pdv.Tests.Infrastructure.Repositories;

public class AdicionalRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly AdicionalRepository _repository;

    public AdicionalRepositoryTests()
    {
        var uniqueDbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: uniqueDbName)
            .Options;

        _context = new AppDbContext(options);
        _repository = new AdicionalRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddItemAdicional()
    {
        var itemAdicional = new ItemAdicional
        {
            Id = Guid.NewGuid(),
            Nome = "Test Adicional",
            Preco = 10.0m
        };

        await _repository.AddAsync(itemAdicional);

        var addedItem = await _repository.GetByIdAsync(itemAdicional.Id);
        Assert.NotNull(addedItem);
        Assert.Equal(itemAdicional.Nome, addedItem?.Nome);
        Assert.Equal(itemAdicional.Preco, addedItem?.Preco);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedResults()
    {
        var itemAdicional1 = new ItemAdicional
        {
            Id = Guid.NewGuid(),
            Nome = "Test Adicional 1",
            Preco = 10.0m
        };
        var itemAdicional2 = new ItemAdicional
        {
            Id = Guid.NewGuid(),
            Nome = "Test Adicional 2",
            Preco = 20.0m
        };
        var itemAdicional3 = new ItemAdicional
        {
            Id = Guid.NewGuid(),
            Nome = "Test Adicional 3",
            Preco = 30.0m
        };
        var itemAdicional4 = new ItemAdicional
        {
            Id = Guid.NewGuid(),
            Nome = "Test Adicional 4",
            Preco = 40.0m
        };
        var itemAdicional5 = new ItemAdicional
        {
            Id = Guid.NewGuid(),
            Nome = "Test Adicional 5",
            Preco = 50.0m
        };

        await _repository.AddAsync(itemAdicional1);
        await _repository.AddAsync(itemAdicional2);
        await _repository.AddAsync(itemAdicional3);
        await _repository.AddAsync(itemAdicional4);
        await _repository.AddAsync(itemAdicional5);

        var results = await _repository.GetAllAsync(1, 2);
        Assert.Equal(2, results.Count());
    }


    [Fact]
    public async Task GetByNameAsync_ShouldReturnItemAdicional()
    {
        var itemAdicional = new ItemAdicional
        {
            Id = Guid.NewGuid(),
            Nome = "Test Name".ToUpper(),
            Preco = 10.0m
        };

        await _repository.AddAsync(itemAdicional);

        var result = await _repository.GetByNameAsync("Test Name".ToUpper());
        Assert.NotNull(result);
        Assert.Equal(itemAdicional.Id, result?.Id);
    }


    [Fact]
    public async Task DeleteAsync_ShouldRemoveItemAdicional()
    {
        var itemAdicional = new ItemAdicional
        {
            Id = Guid.NewGuid(),
            Nome = "Test Adicional",
            Preco = 10.0m
        };

        await _repository.AddAsync(itemAdicional);
        await _repository.DeleteAsync(itemAdicional);

        var deletedItem = await _repository.GetByIdAsync(itemAdicional.Id);
        Assert.Null(deletedItem);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateItemAdicional()
    {
        var itemAdicional = new ItemAdicional
        {
            Id = Guid.NewGuid(),
            Nome = "Old Name",
            Preco = 10.0m
        };

        await _repository.AddAsync(itemAdicional);

        itemAdicional.Nome = "New Name";
        itemAdicional.Preco = 15.0m;
        await _repository.UpdateAsync(itemAdicional);

        var updatedItem = await _repository.GetByIdAsync(itemAdicional.Id);
        Assert.Equal("New Name", updatedItem?.Nome);
        Assert.Equal(15.0m, updatedItem?.Preco);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
