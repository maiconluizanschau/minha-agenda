using System;
using Agenda.Api.Application.Dtos;
using MediatR;

namespace Agenda.Api.Application.Cqrs.Contacts.Commands
{
    public record UpdateContactCommand(Guid Id, UpdateContactDto Dto) : IRequest<ContactDto?>;
}
