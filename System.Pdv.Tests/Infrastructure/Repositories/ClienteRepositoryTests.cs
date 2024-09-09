using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Pdv.Core.Entities;
using System.Pdv.Infrastructure.Data;
using System.Pdv.Infrastructure.Repositories;

namespace System.Pdv.Tests.Infrastructure.Repositories;

public class ClienteRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly ClienteRepository _repository;

    public ClienteRepositoryTests()
    {
        var uniqueDbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: uniqueDbName)
            .Options;

        _context = new AppDbContext(options);
        _repository = new ClienteRepository(_context);

        InitializeData();
    }

    private void InitializeData()
    {
        var cliente1 = new Cliente { Id = Guid.NewGuid(), Nome = "João Silva" };
        var cliente2 = new Cliente { Id = Guid.NewGuid(), Nome = "Maria Oliveira" };
        var cliente3 = new Cliente { Id = Guid.NewGuid(), Nome = "Joana Silva" };

        _context.Clientes.AddRange(cliente1, cliente2, cliente3);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedClientes()
    {
        _context.Clientes.RemoveRange(_context.Clientes);
        var result = await _repository.GetAllAsync(1, 2);

        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCliente()
    {
        var cliente = await _context.Clientes.FirstAsync();
        var result = await _repository.GetByIdAsync(cliente.Id);

        Assert.NotNull(result);
        Assert.Equal(cliente.Id, result.Id);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnClientesByName()
    {
        var result = await _repository.GetByNameAsync("Silva");

        Assert.NotEmpty(result);
        Assert.Contains(result, c => c.Nome.Contains("João Silva"));
        Assert.Contains(result, c => c.Nome.Contains("Joana Silva"));
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveCliente()
    {
        var cliente = await _context.Clientes.FirstAsync();
        await _repository.DeleteAsync(cliente);

        var result = await _repository.GetByIdAsync(cliente.Id);
        Assert.Null(result);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
