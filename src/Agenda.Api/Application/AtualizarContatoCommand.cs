using Agenda.Api.Models;
using MediatR;

namespace Agenda.Api.Application;

public record AtualizarContatoCommand(Guid Id, ContatoCreateUpdateDto Contato) : IRequest<ContatoDto>;
