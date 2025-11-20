
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Agenda.Api.Infrastructure.Messaging
{
    public class RabbitMqPublisher : IRabbitMqPublisher
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMqPublisher> _logger;

        public RabbitMqPublisher(IConfiguration configuration, ILogger<RabbitMqPublisher> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public Task PublishAsync(string queueName, string message, CancellationToken cancellationToken = default)
        {
            var rabbitSection = _configuration.GetSection("RabbitMq");
            var hostName = rabbitSection["HostName"] ?? "rabbitmq";
            var userName = rabbitSection["UserName"] ?? "guest";
            var password = rabbitSection["Password"] ?? "guest";
            var portString = rabbitSection["Port"];
            int port = 5672;
            if (!string.IsNullOrWhiteSpace(portString) && int.TryParse(portString, out var parsed))
            {
                port = parsed;
            }

            var factory = new ConnectionFactory
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
                Port = port
            };

            _logger.LogInformation("Publicando mensagem no RabbitMQ. Queue={Queue}, Host={Host}", queueName, hostName);

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: queueName,
                basicProperties: null,
                body: body);

            return Task.CompletedTask;
        }
    }
}
