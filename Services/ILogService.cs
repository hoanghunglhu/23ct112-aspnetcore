using System;

namespace LearnApiNetCore.Services
{
    public interface ILogService
    {
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message, Exception? ex = null);
        void Fatal(string message, Exception? ex = null);

        void LogException(Exception ex, string additionalInfo = "");
    }
}
