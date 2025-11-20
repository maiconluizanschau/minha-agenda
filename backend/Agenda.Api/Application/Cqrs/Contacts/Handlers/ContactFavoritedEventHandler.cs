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
    public class ContactFavoritedEventHandler : INotificationHandler<ContactFavoritedEvent>
    {
        private readonly IRabbitMqPublisher _publisher;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ContactFavoritedEventHandler> _logger;

        public ContactFavoritedEventHandler(
            IRabbitMqPublisher publisher,
            IConfiguration configuration,
            ILogger<ContactFavoritedEventHandler> logger)
        {
            _publisher = publisher;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Handle(ContactFavoritedEvent notification, CancellationToken cancellationToken)
        {
            var queueName = _configuration["RabbitMq:Queues:ContactFavorited"] ?? "agenda.contacts.favorited";
            var payload = JsonSerializer.Serialize(notification.Contact);

            _logger.LogInformation("Processando ContactFavoritedEvent para {Email}", notification.Contact.Email);

            await _publisher.PublishAsync(queueName, payload, cancellationToken);
        }
    }
}
