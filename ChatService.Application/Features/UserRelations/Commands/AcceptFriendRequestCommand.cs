using ChatService.Domain.Entities;
using ChatService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Application.Features.UserRelations.Commands;

public record AcceptFriendRequestCommand( string RequesterId) : IRequest<bool>;

public class AcceptFriendRequestCommandHandler : IRequestHandler<AcceptFriendRequestCommand, bool>
{
    private readonly IUserRelationRepository _userRelationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtRepository _jwtRepository;
    private readonly ILogger<AcceptFriendRequestCommandHandler> _logger;

    public AcceptFriendRequestCommandHandler(IUserRelationRepository userRelationRepository, IUnitOfWork unitOfWork, IJwtRepository jwtRepository,ILogger<AcceptFriendRequestCommandHandler> logger)
    {
        _userRelationRepository = userRelationRepository;
        _unitOfWork = unitOfWork;
        _jwtRepository = jwtRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(AcceptFriendRequestCommand request, CancellationToken cancellationToken)
    {
        string userId = _jwtRepository.GetUserId();
        var relation = await _userRelationRepository.GetRelationAsync(userId, request.RequesterId, cancellationToken);
        if (relation == null || relation.Status != RelationStatus.Pending) {
            _logger.LogInformation("Not found bcs of status");
            return false;
        }
        if (relation.RequestedByUserId == userId) return false;

        relation.UpdateStatus(RelationStatus.Friends, request.RequesterId); 
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
