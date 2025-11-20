using System;

namespace Agenda.Api.Application.Dtos
{
    public class UpdateContactDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
    }
}
