using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatService.Application.Common.DTOs;
using ChatService.Domain.Interfaces;
using MediatR;

namespace ChatService.Application.Features.UserRelations.Queries
{
    public record AllFriendRequestsQuery : IRequest<List<FriendRequest>>;

    public class AllFriendRequestsQueryHandler : IRequestHandler<AllFriendRequestsQuery,List<FriendRequest>>
    {
        private readonly IUserRelationRepository _userRelationRepository;
        private readonly IJwtRepository _jwtRepository;
        public AllFriendRequestsQueryHandler(IUserRelationRepository userRelationRepository, IJwtRepository jwtRepository)
        {
            _jwtRepository = jwtRepository;
            _userRelationRepository = userRelationRepository;
        }
        public async Task<List<FriendRequest>> Handle(AllFriendRequestsQuery request, CancellationToken cancellationToken)
        {
            var userId = _jwtRepository.GetUserId();
            var res = await _userRelationRepository.GetAllRequests(userId);
            if(res == null || res.Count == 0) return new List<FriendRequest>();
            return res.Select(r => new FriendRequest
            {
                UserId = r.UserAId == userId ? r.UserBId : r.UserAId,
                
                
            }).ToList();
        }
    }

}
