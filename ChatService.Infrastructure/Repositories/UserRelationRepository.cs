using ChatService.Domain.Entities;
using ChatService.Domain.Interfaces;

using ChatService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.Repositories;

public class UserRelationRepository : IUserRelationRepository
{
    private readonly ChatDbContext _context;

    public UserRelationRepository(ChatDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UserRelation relation, CancellationToken cancellationToken)
    {
        await _context.UserRelations.AddAsync(relation, cancellationToken);
    }

    public Task RemoveAsync(UserRelation relation, CancellationToken cancellationToken)
    {
        _context.UserRelations.Remove(relation);
        return Task.CompletedTask;
    }

    public async Task<UserRelation?> GetRelationAsync(string userAId, string userBId, CancellationToken cancellationToken)
    {
        return await _context.UserRelations
            .FirstOrDefaultAsync(r => r.UserAId == userAId && r.UserBId == userBId, cancellationToken);
    }

    public async Task<List<UserRelation>> GetFriendsAsync(string userId, CancellationToken cancellationToken)
    {
         return await _context.UserRelations
            .Where(r => (r.UserAId == userId || r.UserBId == userId) && r.Status == RelationStatus.Friends)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(string userAId, string userBId, CancellationToken cancellationToken)
    {
        return await _context.UserRelations
            .AnyAsync(r => r.UserAId == userAId && r.UserBId == userBId, cancellationToken);
    }

    public async Task<List<UserRelation>> GetAllRequests(string userId)
    {
        return await _context.UserRelations.Where(x => x.UserBId == userId).ToListAsync();

    }
}
