using Microsoft.EntityFrameworkCore;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Enums;

namespace System.Pdv.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<ItemAdicional> Adicionais { get; set; }
    public DbSet<ItemPedido> ItensPedidos { get; set; }
    public DbSet<Mesa> Mesas { get; set; }
    public DbSet<MetodoPagamento> MetodoPagamento { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<StatusPedido> StatusPedidos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => new { u.Email })
            .IsUnique();

        modelBuilder.Entity<Role>()
            .HasIndex(r => new { r.Nome })
            .IsUnique();

        modelBuilder.Entity<Categoria>()
            .HasIndex(c => new { c.Nome })
            .IsUnique();

        modelBuilder.Entity<Mesa>()
            .HasIndex(m => new { m.Numero })
            .IsUnique();

        modelBuilder.Entity<Mesa>()
            .Property(m => m.Status)
            .HasDefaultValue(StatusMesa.Livre);
    }
}