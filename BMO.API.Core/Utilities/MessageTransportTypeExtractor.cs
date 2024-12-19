using System;
using System.Text.Json.Nodes;
using BMO.API.Core.Entities.Enums;

public static class MessageTransportTypeExtractor
{
    public static MessageTransportType ExtractMessageTransportType(string message)
    {
        var jsonObject = JsonObject.Parse(message);
        var variables = jsonObject?["Variables"]?.AsObject();

        if (variables != null && variables.TryGetPropertyValue("message_transport_type", out var transportTypeNode))
        {
            var transportTypeString = transportTypeNode?.ToString();

            if (!string.IsNullOrEmpty(transportTypeString))
            {
                return Enum.Parse<MessageTransportType>(transportTypeString, true);
            }
        }

        throw new InvalidOperationException("Message transport type not found in Kafka message.");
    }
}
