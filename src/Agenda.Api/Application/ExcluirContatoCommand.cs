using MediatR;

namespace Agenda.Api.Application;

public record ExcluirContatoCommand(Guid Id) : IRequest<Unit>;
