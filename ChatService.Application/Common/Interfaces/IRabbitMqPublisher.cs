using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Application.Common.Interfaces;

public interface IRabbitMqPublisher
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken);
}
