using System;

namespace Agenda.Api.Application.Dtos
{
    public class ContactDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public bool Favorito { get; set; }
    }
}
