using Agenda.Api.Models;
using MediatR;

namespace Agenda.Api.Application;

public record ListarContatosQuery(string? Filtro) : IRequest<IReadOnlyList<ContatoDto>>;
