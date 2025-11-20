using Agenda.Api.Application.Dtos;
using MediatR;

namespace Agenda.Api.Application.Cqrs.Contacts.Events
{
    public record ContactCreatedEvent(ContactDto Contact) : INotification;
}
