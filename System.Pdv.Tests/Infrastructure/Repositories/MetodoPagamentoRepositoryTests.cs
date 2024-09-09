using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Pdv.Core.Entities;
using System.Pdv.Infrastructure.Data;
using System.Pdv.Infrastructure.Repositories;
using System.Linq;

namespace System.Pdv.Tests.Infrastructure.Repositories;

public class MetodoPagamentoRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly MetodoPagamentoRepository _repository;

    public MetodoPagamentoRepositoryTests()
    {
        var uniqueDbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: uniqueDbName)
            .Options;

        _context = new AppDbContext(options);
        _repository = new MetodoPagamentoRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddMetodoPagamento()
    {
        var metodoPagamento = new MetodoPagamento
        {
            Id = Guid.NewGuid(),
            Nome = "TEST METODO"
        };

        await _repository.AddAsync(metodoPagamento);

        var addedMetodoPagamento = await _repository.GetByIdAsync(metodoPagamento.Id);
        Assert.NotNull(addedMetodoPagamento);
        Assert.Equal(metodoPagamento.Nome.ToUpper(), addedMetodoPagamento?.Nome);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateMetodoPagamento()
    {
        var metodoPagamento = new MetodoPagamento
        {
            Id = Guid.NewGuid(),
            Nome = "Old Name"
        };

        await _repository.AddAsync(metodoPagamento);

        metodoPagamento.Nome = "NEW NAME";
        await _repository.UpdateAsync(metodoPagamento);

        var updatedMetodoPagamento = await _repository.GetByIdAsync(metodoPagamento.Id);
        Assert.Equal("NEW NAME", updatedMetodoPagamento?.Nome);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllMetodoPagamentos()
    {
        _context.MetodoPagamento.RemoveRange(_context.MetodoPagamento);

        var metodoPagamento1 = new MetodoPagamento
        {
            Id = Guid.NewGuid(),
            Nome = "Metodo 1"
        };
        var metodoPagamento2 = new MetodoPagamento
        {
            Id = Guid.NewGuid(),
            Nome = "Metodo 2"
        };

        await _repository.AddAsync(metodoPagamento1);
        await _repository.AddAsync(metodoPagamento2);

        var results = await _repository.GetAllAsync();
        Assert.Equal(2, results.Count());
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnMetodoPagamento()
    {
        var metodoPagamento = new MetodoPagamento
        {
            Id = Guid.NewGuid(),
            Nome = "UNIQUE NAME"
        };

        await _repository.AddAsync(metodoPagamento);

        var foundMetodoPagamento = await _repository.GetByNameAsync("UNIQUE NAME");
        Assert.NotNull(foundMetodoPagamento);
        Assert.Equal(metodoPagamento.Id, foundMetodoPagamento?.Id);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveMetodoPagamento()
    {
        var metodoPagamento = new MetodoPagamento
        {
            Id = Guid.NewGuid(),
            Nome = "To Be Deleted"
        };

        await _repository.AddAsync(metodoPagamento);
        await _repository.DeleteAsync(metodoPagamento);

        var deletedMetodoPagamento = await _repository.GetByIdAsync(metodoPagamento.Id);
        Assert.Null(deletedMetodoPagamento);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
