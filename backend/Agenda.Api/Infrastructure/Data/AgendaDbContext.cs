using Agenda.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Api.Infrastructure.Data
{
    public class AgendaDbContext : DbContext
    {
        public AgendaDbContext(DbContextOptions<AgendaDbContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts => Set<Contact>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var contact = modelBuilder.Entity<Contact>();

            contact.ToTable("Contacts");
            contact.HasKey(c => c.Id);

            contact.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(100);

            contact.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(150);

            contact.Property(c => c.Telefone)
                .IsRequired()
                .HasMaxLength(20);

            contact.Property(c => c.Favorito)
                .HasDefaultValue(false);

            contact.HasIndex(c => c.Email)
                .IsUnique();
        }
    }
}
