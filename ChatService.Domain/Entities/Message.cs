using System;

namespace ChatService.Domain.Entities;

public class Message
{
    public long Id { get; private set; }
    public string ChatId { get; private set; }
    public string SenderUserId { get; private set; }
    public string ReceiverUserId { get; private set; }
    public string Content { get; private set; }
    public bool IsEdited { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime CreatedAt { get; private set; }


    private Message() { }

    public Message(string chatId, string senderUserId, string receiverUserId, string content)
    {
        if (string.IsNullOrWhiteSpace(chatId)) throw new ArgumentNullException(nameof(chatId));
        if (string.IsNullOrWhiteSpace(content)) throw new ArgumentNullException(nameof(content));

        ChatId = chatId;
        SenderUserId = senderUserId;
        ReceiverUserId = receiverUserId;
        Content = content;
        CreatedAt = DateTime.UtcNow;
        IsEdited = false;
        IsDeleted = false;
    }

    public void Edit(string newContent)
    {
        if (IsDeleted) throw new InvalidOperationException("Cannot edit a deleted message.");
        if (string.IsNullOrWhiteSpace(newContent)) throw new ArgumentNullException(nameof(newContent));

        Content = newContent;
        IsEdited = true;
    }

    public void Delete()
    {
        IsDeleted = true;
        Content = "[Deleted]"; 
    }
}
