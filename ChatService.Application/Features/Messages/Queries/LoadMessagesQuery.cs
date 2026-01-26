using ChatService.Application.DTOs;
using ChatService.Domain.Constants;
using ChatService.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Application.Features.Messages.Queries;

public record LoadMessagesQuery(string UserId, string OtherUserId) : IRequest<List<MessageDto>>;

public class LoadMessagesQueryHandler : IRequestHandler<LoadMessagesQuery, List<MessageDto>>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IChatReadStateRepository _readStateRepository;

    public LoadMessagesQueryHandler(IMessageRepository messageRepository, IChatReadStateRepository readStateRepository)
    {
        _messageRepository = messageRepository;
        _readStateRepository = readStateRepository;
    }

    public async Task<List<MessageDto>> Handle(LoadMessagesQuery request, CancellationToken cancellationToken)
    {
        var chatId = ChatIdHelper.GenerateChatId(request.UserId, request.OtherUserId);

       
        var lastReadState = await _readStateRepository.GetStateAsync(chatId, request.UserId, cancellationToken);

        long lastReadId = lastReadState?.LastReadMessageId ?? 0;

        
        var messages = await _messageRepository.GetChatHistoryAsync(chatId, lastReadId, cancellationToken);
        
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
