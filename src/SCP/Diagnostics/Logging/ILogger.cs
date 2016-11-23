using System;

namespace SCP.Diagnostics.Logging
{
    public interface ILogger
    {
        void Log(LogLevel logLevel, string message);

        void Log(LogLevel logLevel, string message, Exception exception);

        void Debug(string message);

        void Info(string message);

        void Warn(string message);

        void Error(string message, Exception exception);

        void Fatal(string message);

        void Fatal(string message, Exception exception);
    }
}