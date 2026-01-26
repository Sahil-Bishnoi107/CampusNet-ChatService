using System;

namespace ChatService.Domain.Entities;

public enum RelationStatus
{
    NotFriends = 0,
    Pending = 1,
    Friends = 2,
    Blocked = 3
}

public class UserRelation
{
    public string Id { get; private set; }
    public string UserAId { get; private set; }
    public string UserBId { get; private set; }
    public RelationStatus Status { get; private set; }
    public string RequestedByUserId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private UserRelation() { }

    public UserRelation(string userAId, string userBId, RelationStatus status, string requestedByUserId)
    {
        Id = Guid.NewGuid().ToString();
        UserAId = userAId;
        UserBId = userBId;
        Status = status;
        RequestedByUserId = requestedByUserId;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(RelationStatus newStatus, string requestedByUserId)
    {
        Status = newStatus;
        RequestedByUserId = requestedByUserId;
    }
}
