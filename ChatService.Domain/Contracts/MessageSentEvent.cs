using System;

namespace ChatService.Domain.Contracts;

public record MessageSentEvent(string ChatId, string SenderUserId, string ReceiverUserId, string Content, DateTime CreatedAt);
