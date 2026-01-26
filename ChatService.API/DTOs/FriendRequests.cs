namespace ChatService.API.DTOs;

public record FriendRequestDto(string TargetUserId);
public record FriendRequestActionDto(string RequesterId);
public record BlockUserDto(string BlockedUserId);
