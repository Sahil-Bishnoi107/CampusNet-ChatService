using System;

namespace ChatService.Domain.Constants;

public static class ChatIdHelper
{
    public static string GenerateChatId(string userA, string userB)
    {
        // Sort lexicographically
        var compare = string.CompareOrdinal(userA, userB);
        var min = compare < 0 ? userA : userB;
        var max = compare < 0 ? userB : userA;
        return $"{min}_{max}";
    }
}
