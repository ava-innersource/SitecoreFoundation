using System;
using System.Web;

namespace SF.Foundation.Resources
{
    public class LessLogger : dotless.Core.Loggers.ILogger
    {
        public const string LESS_LOGGER_KEY = "SF_LessLogger";

        public LessLogger()
        {
            HttpContext.Current.Items[LESS_LOGGER_KEY] = string.Empty;
        }

        private static void LogMessage(string message)
        {
            HttpContext.Current.Items[LESS_LOGGER_KEY] = message;
        }

        public void Debug(string message, params object[] args)
        {
            LogMessage(message);
        }

        public void Debug(string message)
        {
            LogMessage(message);
        }

        public void Error(string message, params object[] args)
        {
            LogMessage(message);
        }

        public void Error(string message)
        {
            LogMessage(message);
        }

        public void Info(string message, params object[] args)
        {
            LogMessage(message);
        }

        public void Info(string message)
        {
            LogMessage(message);
        }

        public void Log(dotless.Core.Loggers.LogLevel level, string message)
        {
            LogMessage(message);
        }

        public void Warn(string message, params object[] args)
        {
            LogMessage(message);
        }

        public void Warn(string message)
        {
            LogMessage(message);
        }
    }
}
