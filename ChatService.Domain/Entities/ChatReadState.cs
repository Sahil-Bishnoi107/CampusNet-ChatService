using System;

namespace ChatService.Domain.Entities;

public class ChatReadState
{
    public string Id { get; private set; }
    public string ChatId { get; private set; }
    public string UserId { get; private set; }
    public long LastReadMessageId { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private ChatReadState() { }

    public ChatReadState(string chatId, string userId, long lastReadMessageId)
    {
        if (string.IsNullOrWhiteSpace(chatId)) throw new ArgumentNullException(nameof(chatId));

        Id = Guid.NewGuid().ToString();
        ChatId = chatId;
        UserId = userId;
        LastReadMessageId = lastReadMessageId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateLastRead(long messageId)
    {
        if (messageId > LastReadMessageId)
        {
            LastReadMessageId = messageId;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
