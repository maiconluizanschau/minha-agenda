using Agenda.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Api.Data;

public class ContatoRepository : IContatoRepository
{
    private readonly AgendaDbContext _db;

    public ContatoRepository(AgendaDbContext db)
    {
        _db = db;
    }

    public async Task<Contato?> ObterPorIdAsync(Guid id)
    {
        return await _db.Contatos.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IReadOnlyList<Contato>> ListarAsync(string? filtro)
    {
        var query = _db.Contatos.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(filtro))
        {
            query = query.Where(c =>
                c.Nome.Contains(filtro) ||
                c.Email.Contains(filtro) ||
                c.Telefone.Contains(filtro));
        }

        return await query
            .OrderBy(c => c.Nome)
            .ToListAsync();
    }

    public async Task AdicionarAsync(Contato contato)
    {
        await _db.Contatos.AddAsync(contato);
    }

    public Task AtualizarAsync(Contato contato)
    {
        _db.Contatos.Update(contato);
        return Task.CompletedTask;
    }

    public Task RemoverAsync(Contato contato)
    {
        _db.Contatos.Remove(contato);
        return Task.CompletedTask;
    }

    public async Task<bool> EmailExisteAsync(string email, Guid? ignorarId = null)
    {
        var query = _db.Contatos.AsQueryable().Where(c => c.Email == email);
        if (ignorarId.HasValue)
        {
            query = query.Where(c => c.Id != ignorarId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
