using Microsoft.EntityFrameworkCore;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;
using System.Pdv.Infrastructure.Data;
using System.Pdv.Infrastructure.Repositories;
using Xunit;

namespace System.Pdv.Infrastructure.Tests;
public class PedidoRepositoryTests : IDisposable
{
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;
    private readonly PedidoRepository _repository;

    public PedidoRepositoryTests()
    {
        var uniqueDbName = Guid.NewGuid().ToString();
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: uniqueDbName)
            .Options;

        var context = new AppDbContext(_dbContextOptions);
        _repository = new PedidoRepository(context);
    }

    private AppDbContext CreateContext()
    {
        return new AppDbContext(_dbContextOptions);
    }

    private async Task SeedDatabaseAsync()
    {
        using (var context = CreateContext())
        {
            var statusPendente = new StatusPedido { Id = Guid.NewGuid(), Status = "PENDENTE" };
            var statusConcluido = new StatusPedido { Id = Guid.NewGuid(), Status = "CONCLUÍDO" };

            var cliente = new Cliente
            {
                Id = Guid.NewGuid(),
                Nome = "Cliente Teste",
                Telefone = "123456789"
            };

            var garcom = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = "Garçom Teste",
                Email = "garcom@teste.com",
                Role = new Role { Id = Guid.NewGuid(), Nome = "Role Teste" }
            };

            var metodoPagamento = new MetodoPagamento
            {
                Id = Guid.NewGuid(),
                Nome = "Dinheiro"
            };

            context.Pedidos.Add(new Pedido
            {
                Id = Guid.NewGuid(),
                TipoPedido = TipoPedido.Interno,
                StatusPedido = statusPendente,
                DataHora = DateTime.UtcNow,
                Cliente = cliente,
                Garcom = garcom,
                MetodoPagamento = metodoPagamento
            });

            context.Pedidos.Add(new Pedido
            {
                Id = Guid.NewGuid(),
                TipoPedido = TipoPedido.Externo,
                StatusPedido = statusConcluido,
                DataHora = DateTime.UtcNow,
                Cliente = cliente,
                Garcom = garcom,
                MetodoPagamento = metodoPagamento
            });

            await context.SaveChangesAsync();
        }
    }

    [Fact]
    public async Task GetPedidosAsync_ShouldReturnFilteredPedidos()
    {
        await SeedDatabaseAsync();

        using (var context = CreateContext())
        {
            var repository = new PedidoRepository(context);

            var pedidos = await repository.GetPedidosAsync(1, 10, "interno", "PENDENTE");

            Assert.Single(pedidos);

            var pedido = pedidos.First();
            Assert.Equal(TipoPedido.Interno, pedido.TipoPedido);
            Assert.Equal("PENDENTE", pedido.StatusPedido.Status);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnPedido()
    {
        Pedido addedPedido;
        using (var context = CreateContext())
        {
            var repository = new PedidoRepository(context);
            addedPedido = new Pedido
            {
                Id = Guid.NewGuid(),
                TipoPedido = TipoPedido.Interno,
                StatusPedido = new StatusPedido { Id = Guid.NewGuid(), Status = "PENDENTE" },
                DataHora = DateTime.UtcNow,
                Cliente = new Cliente { Id = Guid.NewGuid(), Nome = "Cliente Teste", Telefone = "123456789" },
                Garcom = new Usuario { Id = Guid.NewGuid(), Nome = "Garçom Teste", Email = "garcom@teste.com", Role = new Role { Id = Guid.NewGuid(), Nome = "Role Teste" } },
                MetodoPagamento = new MetodoPagamento { Id = Guid.NewGuid(), Nome = "Dinheiro" }
            };

            context.Pedidos.Add(addedPedido);
            await context.SaveChangesAsync();
        }

        using (var context = CreateContext())
        {
            var repository = new PedidoRepository(context);

            var pedido = await repository.GetByIdAsync(addedPedido.Id);

            Assert.NotNull(pedido);
            Assert.Equal(addedPedido.Id, pedido.Id);
            Assert.Equal(addedPedido.TipoPedido, pedido.TipoPedido);
            Assert.Equal(addedPedido.StatusPedido.Status, pedido.StatusPedido.Status);
        }
    }

    [Fact]
    public async Task GetPedidosAsync_ShouldPaginateResultsCorrectly()
    {
        await SeedDatabaseAsync();

        using (var context = CreateContext())
        {
            var repository = new PedidoRepository(context);

            var pedidosPage1 = await repository.GetPedidosAsync(1, 1, null, null);
            var pedidosPage2 = await repository.GetPedidosAsync(2, 1, null, null);

            Assert.Single(pedidosPage1);
            Assert.Single(pedidosPage2);
            Assert.NotEqual(pedidosPage1.First().Id, pedidosPage2.First().Id);
        }
    }

    [Fact]
    public async Task GetPedidosAsync_ShouldFilterByTipoPedido()
    {
        await SeedDatabaseAsync();

        using (var context = CreateContext())
        {
            var repository = new PedidoRepository(context);

            var pedidosInternos = await repository.GetPedidosAsync(1, 10, "interno", null);
            var pedidosExternos = await repository.GetPedidosAsync(1, 10, "externo", null);

            Assert.Single(pedidosInternos);
            Assert.Equal(TipoPedido.Interno, pedidosInternos.First().TipoPedido);

            Assert.Single(pedidosExternos);
            Assert.Equal(TipoPedido.Externo, pedidosExternos.First().TipoPedido);
        }
    }

    [Fact]
    public async Task GetPedidosAsync_ShouldFilterByStatusPedido()
    {
        await SeedDatabaseAsync();

        using (var context = CreateContext())
        {
            var repository = new PedidoRepository(context);

            var pedidosPendentes = await repository.GetPedidosAsync(1, 10, null, "PENDENTE");
            var pedidosConcluidos = await repository.GetPedidosAsync(1, 10, null, "CONCLUÍDO");

            Assert.Single(pedidosPendentes);
            Assert.Equal("PENDENTE", pedidosPendentes.First().StatusPedido.Status);

            Assert.Single(pedidosConcluidos);
            Assert.Equal("CONCLUÍDO", pedidosConcluidos.First().StatusPedido.Status);
        }
    }

    [Fact]
    public async Task AddAndUpdatePedidoAsync()
    {
        using (var context = CreateContext())
        {
            var repository = new PedidoRepository(context);

            var pedido = new Pedido
            {
                Id = Guid.NewGuid(),
                TipoPedido = TipoPedido.Interno,
                StatusPedido = new StatusPedido { Id = Guid.NewGuid(), Status = "PENDENTE" },
                DataHora = DateTime.UtcNow,
                Cliente = new Cliente { Id = Guid.NewGuid(), Nome = "Cliente Teste", Telefone = "123456789" },
                Garcom = new Usuario { Id = Guid.NewGuid(), Nome = "Garçom Teste", Email = "garcom@teste.com", Role = new Role { Id = Guid.NewGuid(), Nome = "Role Teste" } },
                MetodoPagamento = new MetodoPagamento { Id = Guid.NewGuid(), Nome = "Dinheiro" }
            };

            await repository.AddAsync(pedido);
            var addedPedido = await repository.GetByIdAsync(pedido.Id);

            Assert.NotNull(addedPedido);
            Assert.Equal(pedido.Id, addedPedido.Id);

            addedPedido.StatusPedido.Status = "CONCLUÍDO";
            await repository.UpdateAsync(addedPedido);
            var updatedPedido = await repository.GetByIdAsync(addedPedido.Id);

            Assert.NotNull(updatedPedido);
            Assert.Equal("CONCLUÍDO", updatedPedido.StatusPedido.Status);
        }
    }

    [Fact]
    public async Task DeletePedidoAsync()
    {
        Pedido pedidoToDelete;
        using (var context = CreateContext())
        {
            var repository = new PedidoRepository(context);

            pedidoToDelete = new Pedido
            {
                Id = Guid.NewGuid(),
                TipoPedido = TipoPedido.Interno,
                StatusPedido = new StatusPedido { Id = Guid.NewGuid(), Status = "PENDENTE" },
                DataHora = DateTime.UtcNow,
                Cliente = new Cliente { Id = Guid.NewGuid(), Nome = "Cliente Teste", Telefone = "123456789" },
                Garcom = new Usuario { Id = Guid.NewGuid(), Nome = "Garçom Teste", Email = "garcom@teste.com", Role = new Role { Id = Guid.NewGuid(), Nome = "Role Teste" } },
                MetodoPagamento = new MetodoPagamento { Id = Guid.NewGuid(), Nome = "Dinheiro" }
            };

            await repository.AddAsync(pedidoToDelete);
            var addedPedido = await repository.GetByIdAsync(pedidoToDelete.Id);

            Assert.NotNull(addedPedido);

            await repository.DeleteAsync(pedidoToDelete);
            var deletedPedido = await repository.GetByIdAsync(pedidoToDelete.Id);

            Assert.Null(deletedPedido);
        }
    }

    public void Dispose()
    {
        using (var context = CreateContext())
        {
            context.Database.EnsureDeleted();
        }
    }
}
