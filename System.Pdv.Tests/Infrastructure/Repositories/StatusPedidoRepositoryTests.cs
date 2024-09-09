using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Pdv.Core.Entities;
using System.Pdv.Infrastructure.Data;
using System.Pdv.Infrastructure.Repositories;

namespace System.Pdv.Tests.Infrastructure.Repositories;

public class StatusPedidoRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly StatusPedidoRepository _repository;

    public StatusPedidoRepositoryTests()
    {
        var uniqueDbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: uniqueDbName)
            .Options;

        _context = new AppDbContext(options);
        _repository = new StatusPedidoRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddStatusPedido()
    {
        var statusPedido = new StatusPedido
        {
            Id = Guid.NewGuid(),
            Status = "TEST STATUS"
        };

        await _repository.AddAsync(statusPedido);

        var addedStatusPedido = await _repository.GetByIdAsync(statusPedido.Id);
        Assert.NotNull(addedStatusPedido);
        Assert.Equal(statusPedido.Id, addedStatusPedido?.Id);
        Assert.Equal("TEST STATUS", addedStatusPedido?.Status);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateStatusPedido()
    {
        var statusPedido = new StatusPedido
        {
            Id = Guid.NewGuid(),
            Status = "INITIAL STATUS"
        };

        await _repository.AddAsync(statusPedido);

        statusPedido.Status = "UPDATED STATUS";
        await _repository.UpdateAsync(statusPedido);

        var updatedStatusPedido = await _repository.GetByIdAsync(statusPedido.Id);
        Assert.NotNull(updatedStatusPedido);
        Assert.Equal("UPDATED STATUS", updatedStatusPedido?.Status);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllStatusPedidos()
    {
        _context.StatusPedidos.RemoveRange(_context.StatusPedidos);
        var statusPedido1 = new StatusPedido
        {
            Id = Guid.NewGuid(),
            Status = "STATUS 1"
        };
        var statusPedido2 = new StatusPedido
        {
            Id = Guid.NewGuid(),
            Status = "STATUS 2"
        };

        await _repository.AddAsync(statusPedido1);
        await _repository.AddAsync(statusPedido2);

        var results = await _repository.GetAllAsync();
        Assert.Equal(2, results.Count());
        Assert.Contains(results, s => s.Status == "STATUS 1");
        Assert.Contains(results, s => s.Status == "STATUS 2"); 
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnStatusPedidoByName()
    {
        var statusPedido = new StatusPedido
        {
            Id = Guid.NewGuid(),
            Status = "TEST STATUS"
        };

        await _repository.AddAsync(statusPedido);

        var result = await _repository.GetByNameAsync("TEST STATUS");
        Assert.NotNull(result);
        Assert.Equal("TEST STATUS", result?.Status);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveStatusPedido()
    {
        var statusPedido = new StatusPedido
        {
            Id = Guid.NewGuid(),
            Status = "Test Status"
        };

        await _repository.AddAsync(statusPedido);

        await _repository.DeleteAsync(statusPedido);

        var deletedStatusPedido = await _repository.GetByIdAsync(statusPedido.Id);
        Assert.Null(deletedStatusPedido);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
