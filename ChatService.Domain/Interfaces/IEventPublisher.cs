using System.Threading;
using System.Threading.Tasks;
using ChatService.Domain.Entities;

namespace ChatService.Domain.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync(Message message, CancellationToken cancellationToken);
}
