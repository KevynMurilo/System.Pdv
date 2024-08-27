using Microsoft.EntityFrameworkCore;
using System.Pdv.Core.Entities;

namespace System.Pdv.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Admin> Admins { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Garcom> Garcons { get; set; }
    public DbSet<ItemAdicional> Adicionais { get; set; }
    public DbSet<ItemPedido> ItensPedidos { get; set; }
    public DbSet<Mesa> Mesas { get; set; }
    public DbSet<MetodoPagamento> MetodoPagamento { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<StatusPedido> StatusPedidos { get; set; }
}