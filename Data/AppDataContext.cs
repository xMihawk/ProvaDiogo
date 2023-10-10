using Microsoft.EntityFrameworkCore;
using ProvaDiogo.Models;

namespace ProvaDiogo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        // Entidades relacionadas a funcionários
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<FolhaPagamento> FolhasPagamento { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurações de entidade, chaves primárias, índices, etc.

            modelBuilder.Entity<Funcionario>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<FolhaPagamento>()
                .HasKey(fp => fp.Id);

           
        }
      
    }
}
