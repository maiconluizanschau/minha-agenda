using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Agenda.Api.Application.Cqrs.Contacts.Events;
using Agenda.Api.Infrastructure.Messaging;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Agenda.Api.Application.Cqrs.Contacts.Handlers
{
    public class ContactCreatedEventHandler : INotificationHandler<ContactCreatedEvent>
    {
        private readonly IRabbitMqPublisher _publisher;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ContactCreatedEventHandler> _logger;

        public ContactCreatedEventHandler(
            IRabbitMqPublisher publisher,
            IConfiguration configuration,
            ILogger<ContactCreatedEventHandler> logger)
        {
            _publisher = publisher;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Handle(ContactCreatedEvent notification, CancellationToken cancellationToken)
        {
            var queueName = _configuration["RabbitMq:Queues:ContactCreated"] ?? "agenda.contacts.created";
            var payload = JsonSerializer.Serialize(notification.Contact);

            _logger.LogInformation("Processando ContactCreatedEvent para {Email}", notification.Contact.Email);

            await _publisher.PublishAsync(queueName, payload, cancellationToken);
        }
    }
}
