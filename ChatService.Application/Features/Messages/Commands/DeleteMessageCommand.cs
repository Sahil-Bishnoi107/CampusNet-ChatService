using ChatService.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Application.Features.Messages.Commands;

public record DeleteMessageCommand(string UserId, long MessageId) : IRequest<bool>;

public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, bool>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMessageCommandHandler(IMessageRepository messageRepository, IUnitOfWork unitOfWork)
    {
        _messageRepository = messageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _messageRepository.GetByIdAsync(request.MessageId, cancellationToken);

        if (message == null) return false;

        if (message.SenderUserId != request.UserId) throw new UnauthorizedAccessException("Not allowed to delete this message.");

        message.Delete();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
