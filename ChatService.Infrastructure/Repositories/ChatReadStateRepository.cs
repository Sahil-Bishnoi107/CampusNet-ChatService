using ChatService.Domain.Entities;
using ChatService.Domain.Interfaces;
using ChatService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.Repositories;

public class ChatReadStateRepository : IChatReadStateRepository
{
    private readonly ChatDbContext _context;

    public ChatReadStateRepository(ChatDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ChatReadState state, CancellationToken cancellationToken)
    {
        await _context.ChatReadStates.AddAsync(state, cancellationToken);
    }

    public async Task<ChatReadState?> GetStateAsync(string chatId, string userId, CancellationToken cancellationToken)
    {
        return await _context.ChatReadStates
            .FirstOrDefaultAsync(s => s.ChatId == chatId && s.UserId == userId, cancellationToken);
    }
}
