namespace Agenda.Api.Domain;

public class Contato
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Telefone { get; private set; } = string.Empty;
    public string? Observacoes { get; private set; }
    public bool Favorito { get; private set; }
    public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;
    public DateTime? AtualizadoEm { get; private set; }

    public Contato(string nome, string email, string telefone, string? observacoes)
    {
        Atualizar(nome, email, telefone, observacoes);
    }

    public void Atualizar(string nome, string email, string telefone, string? observacoes)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Observacoes = observacoes;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void MarcarFavorito()
    {
        Favorito = true;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void DesmarcarFavorito()
    {
        Favorito = false;
        AtualizadoEm = DateTime.UtcNow;
    }
}
