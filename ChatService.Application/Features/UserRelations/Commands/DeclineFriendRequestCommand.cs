using ChatService.Domain.Entities;
using ChatService.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Application.Features.UserRelations.Commands;

public record DeclineFriendRequestCommand(string UserId, string RequesterId) : IRequest<bool>;

public class DeclineFriendRequestCommandHandler : IRequestHandler<DeclineFriendRequestCommand, bool>
{
    private readonly IUserRelationRepository _userRelationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeclineFriendRequestCommandHandler(IUserRelationRepository userRelationRepository, IUnitOfWork unitOfWork)
    {
        _userRelationRepository = userRelationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeclineFriendRequestCommand request, CancellationToken cancellationToken)
    {
        var relation = await _userRelationRepository.GetRelationAsync(request.UserId, request.RequesterId, cancellationToken);
        if (relation == null || relation.Status != RelationStatus.Pending) return false;

        await _userRelationRepository.RemoveAsync(relation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
