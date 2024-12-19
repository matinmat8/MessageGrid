using System.Text.Json.Nodes;
using BMO.API.Core.Entities.Enums;

public static class MessageTypeExtractor
{
    public static MessageType? ExtractMessageType(string message)
    {
        var jsonObject = JsonObject.Parse(message);
        var messageTypeString = jsonObject?["MessageType"]?.ToString();

        if (string.IsNullOrEmpty(messageTypeString))
        {
            return null;
        }

        if (!Enum.TryParse(messageTypeString, true, out MessageType messageType))
        {
            return null;
        }

        return messageType;
    }
}
