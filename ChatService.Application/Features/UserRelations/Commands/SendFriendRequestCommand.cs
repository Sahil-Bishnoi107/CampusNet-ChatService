using ChatService.Domain.Entities;
using ChatService.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Application.Features.UserRelations.Commands;

public record SendFriendRequestCommand(string RequesterId, string TargetUserId) : IRequest<bool>;

public class SendFriendRequestCommandHandler : IRequestHandler<SendFriendRequestCommand, bool>
{
    private readonly IUserRelationRepository _userRelationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SendFriendRequestCommandHandler(IUserRelationRepository userRelationRepository, IUnitOfWork unitOfWork)
    {
        _userRelationRepository = userRelationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(SendFriendRequestCommand request, CancellationToken cancellationToken)
    {
        
        var existing = await _userRelationRepository.GetRelationAsync(request.RequesterId, request.TargetUserId, cancellationToken);
        if (existing != null)
        {
             if (existing.Status == RelationStatus.Blocked || existing.Status == RelationStatus.Friends) return false;
             if (existing.Status == RelationStatus.Pending) return false;
        }

       
        var compare = string.CompareOrdinal(request.RequesterId, request.TargetUserId);
        var userA = compare < 0 ? request.RequesterId : request.TargetUserId;
        var userB = compare < 0 ? request.TargetUserId : request.RequesterId;

        var relation = new UserRelation(userA, userB, RelationStatus.Pending, request.RequesterId);
        await _userRelationRepository.AddAsync(relation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true; 
    }
}
