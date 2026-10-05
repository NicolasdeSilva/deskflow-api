using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Chamado> Chamados => Set<Chamado>();
    public DbSet<Interacao> Interacoes => Set<Interacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Nome).IsRequired().HasMaxLength(100);
            entity.HasIndex(c => c.Nome).IsUnique();

            entity.HasData(
                new Categoria { Id = 1, Nome = "Hardware" },
                new Categoria { Id = 2, Nome = "Software" },
                new Categoria { Id = 3, Nome = "Redes" },
                new Categoria { Id = 4, Nome = "Gestão de Acessos" });
        });

        modelBuilder.Entity<Chamado>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Titulo).IsRequired().HasMaxLength(150);
            entity.Property(c => c.Descricao).IsRequired().HasMaxLength(2000);
            entity.Property(c => c.SolicitanteNome).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Solucao).HasMaxLength(2000);

            // Categoria 1:N Chamado (Restrict: não apaga categoria que tem chamados)
            entity.HasOne(c => c.Categoria)
                  .WithMany(cat => cat.Chamados)
                  .HasForeignKey(c => c.CategoriaId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Interacao>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Autor).IsRequired().HasMaxLength(100);
            entity.Property(i => i.Mensagem).IsRequired().HasMaxLength(2000);

            // Chamado 1:N Interacao
            entity.HasOne(i => i.Chamado)
                  .WithMany(c => c.Interacoes)
                  .HasForeignKey(i => i.ChamadoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
