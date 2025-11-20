using System;
using Agenda.Api.Application.Dtos;
using MediatR;

namespace Agenda.Api.Application.Cqrs.Contacts.Events
{
    public record ContactFavoritedEvent(ContactDto Contact) : INotification;
}
