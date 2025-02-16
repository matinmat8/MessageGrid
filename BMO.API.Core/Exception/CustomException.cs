using System;

public class CustomException : Exception
{
    public int MessageKey { get; }

    public CustomException(int messageKey) : base()
    {
        MessageKey = messageKey;
    }

    public CustomException(int messageKey, string message) : base(message)
    {
        MessageKey = messageKey;
    }

    public CustomException(int messageKey, string message, Exception innerException) : base(message, innerException)
    {
        MessageKey = messageKey;
    }
}