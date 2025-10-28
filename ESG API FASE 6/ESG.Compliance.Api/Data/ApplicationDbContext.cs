using ESG.Compliance.Api.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ESG.Compliance.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<LicencaAmbiental> LicencasAmbientais { get; set; }
        public DbSet<AuditoriaAmbiental> AuditoriasAmbientais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade LicencaAmbiental
            modelBuilder.Entity<LicencaAmbiental>(entity =>
            {
                // Define o nome da tabela no banco de dados
                entity.ToTable("LicencasAmbientais");

                // Define a chave primária
                entity.HasKey(l => l.Id);

                // Configura a propriedade NomeDaLicenca
                entity.Property(l => l.NomeDaLicenca)
                    .IsRequired() // Campo obrigatório
                    .HasMaxLength(200); // Tamanho máximo

                // Configura a propriedade OrgaoEmissor
                entity.Property(l => l.OrgaoEmissor)
                    .IsRequired()
                    .HasMaxLength(150);

                // Configura a propriedade Status
                entity.Property(l => l.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            // Configuração da entidade AuditoriaAmbiental
            modelBuilder.Entity<AuditoriaAmbiental>(entity =>
            {
                // Define o nome da tabela no banco de dados
                entity.ToTable("AuditoriasAmbientais");

                // Define a chave primária
                entity.HasKey(a => a.Id);

                // Configura a propriedade Titulo
                entity.Property(a => a.Titulo)
                    .IsRequired()
                    .HasMaxLength(250);

                // Configura a propriedade AuditorResponsavel
                entity.Property(a => a.AuditorResponsavel)
                    .IsRequired()
                    .HasMaxLength(150);

                // Configura a propriedade Status
                entity.Property(a => a.Status)
                   .IsRequired()
                   .HasMaxLength(50);
            });
        }
    }
}
