using Agenda.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Api.Data;

public class AgendaDbContext : DbContext
{
    public AgendaDbContext(DbContextOptions<AgendaDbContext> options) : base(options)
    {
    }

    public DbSet<Contato> Contatos => Set<Contato>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contato>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).HasMaxLength(150).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Telefone).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Observacoes).HasMaxLength(500);
            entity.Property(e => e.Favorito).HasDefaultValue(false);
            entity.Property(e => e.CriadoEm).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        base.OnModelCreating(modelBuilder);
    }
}
