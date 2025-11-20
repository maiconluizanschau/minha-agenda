using Agenda.Api.Domain.Entities;
using Agenda.Api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Api.Infrastructure.Data.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly AgendaDbContext _context;

        public ContactRepository(AgendaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return await _context.Contacts
                .Where(c => c.Ativo)
                .OrderByDescending(c => c.Favorito)
                .ThenBy(c => c.Nome)
                .ToListAsync();
        }

        public async Task<Contact?> GetByIdAsync(Guid id)
        {
            return await _context.Contacts.FindAsync(id);
        }

        public async Task<Contact?> GetByEmailAsync(string email)
        {
            return await _context.Contacts.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task AddAsync(Contact contact)
        {
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Contact contact)
        {
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
        }
    }
}
