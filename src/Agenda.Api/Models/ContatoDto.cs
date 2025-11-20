namespace Agenda.Api.Models;

public class ContatoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
    public bool Favorito { get; set; }
}
