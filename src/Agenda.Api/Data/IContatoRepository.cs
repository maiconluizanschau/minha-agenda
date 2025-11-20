using Agenda.Api.Domain;

namespace Agenda.Api.Data;

public interface IContatoRepository
{
    Task<Contato?> ObterPorIdAsync(Guid id);
    Task<IReadOnlyList<Contato>> ListarAsync(string? filtro);
    Task AdicionarAsync(Contato contato);
    Task AtualizarAsync(Contato contato);
    Task RemoverAsync(Contato contato);
    Task<bool> EmailExisteAsync(string email, Guid? ignorarId = null);
    Task SaveChangesAsync();
}
