using Agenda.Api.Domain.Entities;

namespace Agenda.Api.Domain.Interfaces
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetAllAsync();
        Task<Contact?> GetByIdAsync(Guid id);
        Task<Contact?> GetByEmailAsync(string email);
        Task AddAsync(Contact contact);
        Task UpdateAsync(Contact contact);
    }
}
