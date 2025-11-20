using System;
using MediatR;

namespace Agenda.Api.Application.Cqrs.Contacts.Commands
{
    public record DeleteContactCommand(Guid Id) : IRequest<bool>;
}
