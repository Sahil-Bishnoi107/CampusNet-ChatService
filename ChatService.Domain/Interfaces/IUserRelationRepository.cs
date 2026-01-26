
using ChatService.Domain.Entities;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Domain.Interfaces;

public interface IUserRelationRepository
{
    Task AddAsync(UserRelation relation, CancellationToken cancellationToken);
    Task RemoveAsync(UserRelation relation, CancellationToken cancellationToken);
    Task<UserRelation?> GetRelationAsync(string userAId, string userBId, CancellationToken cancellationToken);
    Task<List<UserRelation>> GetFriendsAsync(string userId, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string userAId, string userBId, CancellationToken cancellationToken);

    Task<List<UserRelation>> GetAllRequests(string userId);
}
