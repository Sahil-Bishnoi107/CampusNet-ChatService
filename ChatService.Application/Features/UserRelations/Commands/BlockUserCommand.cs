using ChatService.Domain.Entities;
using ChatService.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Application.Features.UserRelations.Commands;

public record BlockUserCommand(string BlockerId, string BlockedId) : IRequest<bool>;

public class BlockUserCommandHandler : IRequestHandler<BlockUserCommand, bool>
{
    private readonly IUserRelationRepository _userRelationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BlockUserCommandHandler(IUserRelationRepository userRelationRepository, IUnitOfWork unitOfWork)
    {
        _userRelationRepository = userRelationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(BlockUserCommand request, CancellationToken cancellationToken)
    {
        var relation = await _userRelationRepository.GetRelationAsync(request.BlockerId, request.BlockedId, cancellationToken);
        if (relation == null)
        {
            var compare = string.CompareOrdinal(request.BlockerId, request.BlockedId);
            var userA = compare < 0 ? request.BlockerId : request.BlockedId;
            var userB = compare < 0 ? request.BlockedId : request.BlockerId;

            relation = new UserRelation(userA, userB, RelationStatus.Blocked, request.BlockerId);
            await _userRelationRepository.AddAsync(relation, cancellationToken);
        }
        else
        {
            relation.UpdateStatus(RelationStatus.Blocked, request.BlockerId);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
