using Microsoft.Extensions.Logging;
using System;
using System.IO;

public class FileLogger : ILogger
{
    private readonly string _filePath;
    private static readonly object _lock = new object();

    public FileLogger(string filePath)
    {
        _filePath = filePath;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var message = formatter(state, exception);
        lock (_lock)
        {
            File.AppendAllText(_filePath, $"{DateTime.Now}: {logLevel.ToString()} - {message}{Environment.NewLine}");
        }
    }
}