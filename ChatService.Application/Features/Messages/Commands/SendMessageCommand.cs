using ChatService.Domain.Constants;
using ChatService.Domain.Contracts;
using ChatService.Domain.Entities;
using ChatService.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Application.Features.Messages.Commands;

public record SendMessageCommand(string SenderUserId, string ReceiverUserId, string Content) : IRequest<long>;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, long>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisher _publisher;

    public SendMessageCommandHandler(IMessageRepository messageRepository, IUnitOfWork unitOfWork, IEventPublisher publisher)
    {
        _messageRepository = messageRepository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task<long> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        
        var chatId = ChatIdHelper.GenerateChatId(request.SenderUserId, request.ReceiverUserId);

       
        var message = new Message(chatId, request.SenderUserId, request.ReceiverUserId, request.Content);

        
        await _messageRepository.AddAsync(message, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        
        var eventMessage = new Message(chatId, request.SenderUserId, request.ReceiverUserId, message.Content);
        await _publisher.PublishAsync(eventMessage, cancellationToken);

        return message.Id;
    }
}
