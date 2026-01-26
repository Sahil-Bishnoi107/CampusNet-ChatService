using ChatService.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Application.Features.Messages.Commands;

public record EditMessageCommand(string UserId, long MessageId, string NewContent) : IRequest<bool>;

public class EditMessageCommandHandler : IRequestHandler<EditMessageCommand, bool>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditMessageCommandHandler(IMessageRepository messageRepository, IUnitOfWork unitOfWork)
    {
        _messageRepository = messageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(EditMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _messageRepository.GetByIdAsync(request.MessageId, cancellationToken);

        if (message == null) return false;

        if (message.SenderUserId != request.UserId) throw new UnauthorizedAccessException("Not allowed to edit this message.");

        message.Edit(request.NewContent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
