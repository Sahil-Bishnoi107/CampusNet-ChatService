using ChatService.Domain.Entities;
using ChatService.Domain.Interfaces;
using ChatService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ChatDbContext _context;

    public MessageRepository(ChatDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Message message, CancellationToken cancellationToken)
    {
        await _context.Messages.AddAsync(message, cancellationToken);
    }

    public async Task<Message?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<List<Message>> GetChatHistoryAsync(string chatId, long lastReadId, CancellationToken cancellationToken)
    {
        return await _context.Messages
            .Where(m => m.ChatId == chatId && m.Id > lastReadId && !m.IsDeleted)
            .OrderBy(m => m.Id)
            .Take(50)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Message>> GetAllChatMessagesAsync(string chatId, CancellationToken cancellationToken)
    {
        return await _context.Messages
            .Where(m => m.ChatId == chatId && !m.IsDeleted)
            .OrderBy(m => m.Id)
            .ToListAsync(cancellationToken);
    }
}
