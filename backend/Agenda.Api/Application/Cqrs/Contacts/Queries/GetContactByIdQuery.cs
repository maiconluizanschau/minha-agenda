using System;
using Agenda.Api.Application.Dtos;
using MediatR;

namespace Agenda.Api.Application.Cqrs.Contacts.Queries
{
    public record GetContactByIdQuery(Guid Id) : IRequest<ContactDto?>;
}
