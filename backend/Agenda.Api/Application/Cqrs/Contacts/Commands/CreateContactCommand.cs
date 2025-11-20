using Agenda.Api.Application.Dtos;
using MediatR;

namespace Agenda.Api.Application.Cqrs.Contacts.Commands
{
    public record CreateContactCommand(CreateContactDto Dto) : IRequest<ContactDto>;
}
