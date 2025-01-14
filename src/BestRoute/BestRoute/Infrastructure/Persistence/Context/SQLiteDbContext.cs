using BestRoute.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BestRoute.Infrastructure.Persistence.Context;

public class SQLiteDbContext : DbContext
{
    public DbSet<Route> Rotas { get; set; }

    public SQLiteDbContext(DbContextOptions<SQLiteDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Route>(entity =>
        {
            entity.ToTable("Rotas");

            entity.HasKey(r => r.Id);

            entity.Property(r => r.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(r => r.Origem)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(r => r.Destino)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(r => r.Custo)
                .IsRequired()
                .HasColumnType("decimal(10, 2)");
        });

        base.OnModelCreating(modelBuilder);
    }

    public void Seed()
    {
        if (!Rotas.Any())
        {
            var rotasIniciais = new List<Route>
                {
                    new() { Origem = "GRU", Destino = "BRC", Custo = 10 },
                    new() { Origem = "BRC", Destino = "SCL", Custo = 5 },
                    new() { Origem = "GRU", Destino = "CDG", Custo = 75 },
                    new() { Origem = "GRU", Destino = "SCL", Custo = 20 },
                    new() { Origem = "GRU", Destino = "ORL", Custo = 56 },
                    new() { Origem = "ORL", Destino = "CDG", Custo = 5 },
                    new() { Origem = "SCL", Destino = "ORL", Custo = 20 }
                };

            Rotas.AddRange(rotasIniciais);
            SaveChanges();
        }
    }
}
