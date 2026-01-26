using ChatService.Application.DTOs;
using ChatService.Domain.Constants;
using ChatService.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Application.Features.Messages.Queries;

public record LoadFullMessagesQuery(string UserId, string OtherUserId) : IRequest<List<MessageDto>>;

public class LoadFullMessagesQueryHandler : IRequestHandler<LoadFullMessagesQuery, List<MessageDto>>
{
    private readonly IMessageRepository _messageRepository;

    public LoadFullMessagesQueryHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<List<MessageDto>> Handle(LoadFullMessagesQuery request, CancellationToken cancellationToken)
    {
        var chatId = ChatIdHelper.GenerateChatId(request.UserId, request.OtherUserId);

        var messages = await _messageRepository.GetAllChatMessagesAsync(chatId, cancellationToken);
        
        return messages.Select(m => new MessageDto
            {
                Id = m.Id,
                SenderUserId = m.SenderUserId,
                Content = m.IsDeleted ? "[Deleted]" : m.Content, 
                CreatedAt = m.CreatedAt,
                IsEdited = m.IsEdited
            })
            .ToList();
    }
}
