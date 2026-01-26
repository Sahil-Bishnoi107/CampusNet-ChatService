using ChatService.Domain.Entities;
using ChatService.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace ChatService.Application.Features.UserRelations.Queries;

public record FriendDto(string FriendUserId, DateTime BecomedFriendsAt);

public record LoadFriendsQuery(string UserId) : IRequest<List<FriendDto>>;

public class LoadFriendsQueryHandler : IRequestHandler<LoadFriendsQuery, List<FriendDto>>
{
    private readonly IUserRelationRepository _userRelationRepository;

    public LoadFriendsQueryHandler(IUserRelationRepository userRelationRepository)
    {
        _userRelationRepository = userRelationRepository;
    }

    public async Task<List<FriendDto>> Handle(LoadFriendsQuery request, CancellationToken cancellationToken)
    {
        var relations = await _userRelationRepository.GetFriendsAsync(request.UserId, cancellationToken);

        return relations.Select(r => new FriendDto(
            r.UserAId == request.UserId ? r.UserBId : r.UserAId,
            r.CreatedAt
        )).ToList();
    }
}
