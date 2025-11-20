using System.Collections.Generic;
using Agenda.Api.Application.Dtos;
using MediatR;

namespace Agenda.Api.Application.Cqrs.Contacts.Queries
{
    public record GetAllContactsQuery() : IRequest<IEnumerable<ContactDto>>;
}
