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
        //Para falar que o valor é unico
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

        modelBuilder.Entity<ItemAdicional>()
            .HasIndex(a => new { a.Nome })
            .IsUnique();

        modelBuilder.Entity<MetodoPagamento>()
            .HasIndex(m => new { m.Nome })
            .IsUnique();

        modelBuilder.Entity<StatusPedido>()
            .HasIndex(o => new { o.Status })
            .IsUnique();

        //Para iniciar com um valor default caso não seja passado
        modelBuilder.Entity<Mesa>()
            .Property(m => m.Status)
            .HasDefaultValue(StatusMesa.Livre);

        //Para quando uma tabela for deletada ao inves de deletar as que tem relacionamento, deixar como null
        modelBuilder.Entity<Produto>()
            .HasOne(p => p.Categoria)
            .WithMany(c => c.Produtos)
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Pedido>()
               .HasOne(p => p.MetodoPagamento)
               .WithMany(m => m.Pedidos)
               .HasForeignKey(p => p.MetodoPagamentoId)
               .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.StatusPedido)
            .WithMany()
            .HasForeignKey(p => p.StatusPedidoId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.Garcom)
            .WithMany(u => u.Pedidos)
            .HasForeignKey(p => p.GarcomId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}