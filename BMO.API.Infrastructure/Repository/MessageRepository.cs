using System.Collections.Generic;

public static class MessageRepository
{
    private static readonly Dictionary<int, string> Messages = new Dictionary<int, string>
    {
        { 1, "User not found." },
        { 2, "Invalid request parameters." },
        { 3, "Access denied." }
    };

    public static string GetMessage(int key)
    {
        return Messages.ContainsKey(key) ? Messages[key] : "Unknown error.";
    }
}