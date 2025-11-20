namespace Agenda.Api.Models;

public class ContatoCreateUpdateDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
}
