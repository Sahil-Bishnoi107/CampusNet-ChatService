using System;

namespace ChatService.Application.DTOs;

public class MessageDto
{
    public long Id { get; set; }
    public string SenderUserId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsEdited { get; set; }
}
