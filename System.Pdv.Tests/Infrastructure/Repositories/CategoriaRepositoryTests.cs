using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Pdv.Core.Entities;
using System.Pdv.Infrastructure.Data;
using System.Pdv.Infrastructure.Repositories;

namespace System.Pdv.Tests.Infrastructure.Repositories;

public class CategoriaRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly CategoriaRepository _repository;

    public CategoriaRepositoryTests()
    {
        var uniqueDbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: uniqueDbName)
            .Options;

        _context = new AppDbContext(options);
        _repository = new CategoriaRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddCategoria()
    {
        var categoria = new Categoria
        {
            Id = Guid.NewGuid(),
            Nome = "TEST CATEGORIA"
        };

        await _repository.AddAsync(categoria);

        var addedCategoria = await _repository.GetByIdAsync(categoria.Id);
        Assert.NotNull(addedCategoria);
        Assert.Equal(categoria.Nome.ToUpper(), addedCategoria?.Nome);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCategoria()
    {
        var categoria = new Categoria
        {
            Id = Guid.NewGuid(),
            Nome = "Old Name"
        };

        await _repository.AddAsync(categoria);

        categoria.Nome = "NEW NAME";
        await _repository.UpdateAsync(categoria);

        var updatedCategoria = await _repository.GetByIdAsync(categoria.Id);
        Assert.Equal("NEW NAME", updatedCategoria?.Nome);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCategorias()
    {
        _context.Categorias.RemoveRange(_context.Categorias);
        await _context.SaveChangesAsync();

        var categoria1 = new Categoria
        {
            Id = Guid.NewGuid(),
            Nome = "Test Categoria 1"
        };
        var categoria2 = new Categoria
        {
            Id = Guid.NewGuid(),
            Nome = "Test Categoria 2"
        };

        await _repository.AddAsync(categoria1);
        await _repository.AddAsync(categoria2);

        var results = await _repository.GetAllAsync();
        Assert.Equal(2, results.Count());
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
