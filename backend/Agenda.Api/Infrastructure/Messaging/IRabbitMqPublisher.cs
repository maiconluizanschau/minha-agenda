using System.Threading;
using System.Threading.Tasks;

namespace Agenda.Api.Infrastructure.Messaging
{
    public interface IRabbitMqPublisher
    {
        Task PublishAsync(string queueName, string message, CancellationToken cancellationToken = default);
    }
}
