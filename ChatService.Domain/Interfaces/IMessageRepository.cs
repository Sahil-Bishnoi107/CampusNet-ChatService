using ChatService.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Domain.Interfaces;

public interface IMessageRepository
{
    Task AddAsync(Message message, CancellationToken cancellationToken);
    Task<Message?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<List<Message>> GetChatHistoryAsync(string chatId, long lastReadId, CancellationToken cancellationToken);
    Task<List<Message>> GetAllChatMessagesAsync(string chatId, CancellationToken cancellationToken);
}
