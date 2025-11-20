using System;
using Agenda.Api.Application.Dtos;
using MediatR;

namespace Agenda.Api.Application.Cqrs.Contacts.Commands
{
    public record ToggleFavoriteContactCommand(Guid Id) : IRequest<ContactDto?>;
}
