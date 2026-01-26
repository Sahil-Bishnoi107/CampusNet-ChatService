using ChatService.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Application.Features.UserRelations.Queries;

public record MutualFriendDto(string FriendUserId);

public record GetMutualFriendsQuery(string UserId, string OtherUserId) : IRequest<List<MutualFriendDto>>;

public class GetMutualFriendsQueryHandler : IRequestHandler<GetMutualFriendsQuery, List<MutualFriendDto>>
{
    private readonly IUserRelationRepository _userRelationRepository;

    public GetMutualFriendsQueryHandler(IUserRelationRepository userRelationRepository)
    {
        _userRelationRepository = userRelationRepository;
    }

    public async Task<List<MutualFriendDto>> Handle(GetMutualFriendsQuery request, CancellationToken cancellationToken)
    {
        // Get friends of User A
        var friendsA = await _userRelationRepository.GetFriendsAsync(request.UserId, cancellationToken);
        var friendIdsA = friendsA.Select(r => r.UserAId == request.UserId ? r.UserBId : r.UserAId).ToHashSet();

        // Get friends of User B (Other User)
        var friendsB = await _userRelationRepository.GetFriendsAsync(request.OtherUserId, cancellationToken);
        var friendIdsB = friendsB.Select(r => r.UserAId == request.OtherUserId ? r.UserBId : r.UserAId).ToHashSet();

        // Intersect
        friendIdsA.IntersectWith(friendIdsB);

        return friendIdsA.Select(id => new MutualFriendDto(id)).ToList();
    }
}
