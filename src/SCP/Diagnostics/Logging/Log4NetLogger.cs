using System;
using log4net;
using log4net.Config;

namespace SCP.Diagnostics.Logging
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _logger;

        public Log4NetLogger()
        {
            _logger = LogManager.GetLogger(AppDomain.CurrentDomain.FriendlyName);
            XmlConfigurator.Configure();
        }

        public Log4NetLogger(string loggerName)
        {
            _logger = LogManager.GetLogger(loggerName);
            XmlConfigurator.Configure();
        }

        public Log4NetLogger(ILog logger)
        {
            _logger = logger;
        }

        public void Log(LogLevel logLevel, string message)
        {
            WrapLog(logLevel, message);
        }

        public void Log(LogLevel logLevel, string message, Exception exception)
        {
            WrapLog(logLevel, message, exception);
        }

        public void Debug(string message)
        {
            WrapLog(LogLevel.Debug, message);
        }

        public void Info(string message)
        {
            WrapLog(LogLevel.Info, message);
        }

        public void Warn(string message)
        {
            WrapLog(LogLevel.Warning, message);
        }

        public void Error(string message, Exception exception)
        {
            WrapLog(LogLevel.Error, message, exception);
        }

        public void Fatal(string message)
        {
            WrapLog(LogLevel.Fatal, message);
        }

        public void Fatal(string message, Exception exception)
        {
            WrapLog(LogLevel.Fatal, message, exception);
        }

        private void WrapLog(LogLevel logLevel, string message, Exception exception = null)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    _logger.Debug(message, exception);
                    break;
                case LogLevel.Error:
                    _logger.Error(message, exception);
                    break;
                case LogLevel.Fatal:
                    _logger.Fatal(message, exception);
                    break;
                case LogLevel.Warning:
                    _logger.Warn(message, exception);
                    break;
                default:
                    _logger.Info(message, exception);
                    break;
            }
        }
    }
}