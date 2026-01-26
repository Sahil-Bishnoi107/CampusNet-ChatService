using ChatService.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Domain.Interfaces;

public interface IChatReadStateRepository
{
    Task<ChatReadState?> GetStateAsync(string chatId, string userId, CancellationToken cancellationToken);
    Task AddAsync(ChatReadState state, CancellationToken cancellationToken);
}
