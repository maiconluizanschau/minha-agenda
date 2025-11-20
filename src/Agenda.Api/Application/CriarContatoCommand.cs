using Agenda.Api.Models;
using MediatR;

namespace Agenda.Api.Application;

public record CriarContatoCommand(ContatoCreateUpdateDto Contato) : IRequest<ContatoDto>;
