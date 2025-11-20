using Agenda.Api.Models;
using MediatR;

namespace Agenda.Api.Application;

public record ObterContatoQuery(Guid Id) : IRequest<ContatoDto?>;
