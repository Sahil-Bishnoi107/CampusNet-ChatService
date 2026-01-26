namespace ChatService.API.DTOs
{
    public record SendMessageRequest(string recieverId,string content);
    public record EditMessageRequest(string content);
}
