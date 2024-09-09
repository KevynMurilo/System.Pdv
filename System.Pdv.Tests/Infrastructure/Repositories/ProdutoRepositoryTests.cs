using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Pdv.Core.Entities;
using System.Pdv.Infrastructure.Data;
using System.Pdv.Infrastructure.Repositories;

namespace System.Pdv.Tests.Infrastructure.Repositories;

public class ProdutoRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly ProdutoRepository _repository;

    public ProdutoRepositoryTests()
    {
        var uniqueDbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: uniqueDbName)
            .Options;

        _context = new AppDbContext(options);
        _repository = new ProdutoRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddProduto()
    {
        var produto = new Produto
        {
            Id = Guid.NewGuid(),
            Nome = "Test Produto",
            Categoria = new Categoria { Id = Guid.NewGuid(), Nome = "Test Categoria" }
        };

        await _repository.AddAsync(produto);

        var addedProduto = await _repository.GetByIdAsync(produto.Id);
        Assert.NotNull(addedProduto);
        Assert.Equal(produto.Id, addedProduto?.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProduto()
    {
        var produto = new Produto
        {
            Id = Guid.NewGuid(),
            Nome = "Test Produto",
            Categoria = new Categoria { Id = Guid.NewGuid(), Nome = "Test Categoria" }
        };

        await _repository.AddAsync(produto);

        produto.Nome = "Updated Produto";
        await _repository.UpdateAsync(produto);

        var updatedProduto = await _repository.GetByIdAsync(produto.Id);
        Assert.NotNull(updatedProduto);
        Assert.Equal("Updated Produto", updatedProduto?.Nome);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnProdutos()
    {
        _context.Produtos.RemoveRange(_context.Produtos);
        var produto1 = new Produto
        {
            Id = Guid.NewGuid(),
            Nome = "Test Produto 1",
            Categoria = new Categoria { Id = Guid.NewGuid(), Nome = "Test Categoria 1" }
        };
        var produto2 = new Produto
        {
            Id = Guid.NewGuid(),
            Nome = "Test Produto 2",
            Categoria = new Categoria { Id = Guid.NewGuid(), Nome = "Test Categoria 2" }
        };

        await _repository.AddAsync(produto1);
        await _repository.AddAsync(produto2);

        var results = await _repository.GetAllAsync(1, 10);
        Assert.Equal(2, results.Count());
    }

    [Fact]
    public async Task GetProdutoByCategoria_ShouldReturnProdutosByCategoria()
    {
        var categoria = new Categoria
        {
            Id = Guid.NewGuid(),
            Nome = "Test Categoria"
        };

        var produto1 = new Produto
        {
            Id = Guid.NewGuid(),
            Nome = "Test Produto 1",
            Categoria = categoria
        };
        var produto2 = new Produto
        {
            Id = Guid.NewGuid(),
            Nome = "Test Produto 2",
            Categoria = categoria
        };

        await _repository.AddAsync(produto1);
        await _repository.AddAsync(produto2);

        var results = await _repository.GetProdutoByCategoria(categoria.Id, 1, 10);
        Assert.Equal(2, results.Count());
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveProduto()
    {
        var produto = new Produto
        {
            Id = Guid.NewGuid(),
            Nome = "Test Produto",
            Categoria = new Categoria { Id = Guid.NewGuid(), Nome = "Test Categoria" }
        };

        await _repository.AddAsync(produto);

        await _repository.DeleteAsync(produto);

        var deletedProduto = await _repository.GetByIdAsync(produto.Id);
        Assert.Null(deletedProduto);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
