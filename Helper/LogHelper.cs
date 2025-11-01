using System;
using log4net;

namespace LearnApiNetCore.Helpers
{
    public static class LogHelper
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LogHelper));

        public static void LogException(Exception ex, string context = null)
        {
            if (ex == null) return;

            var message = string.IsNullOrWhiteSpace(context)
                ? "Unhandled exception"
                : $"Exception in {context}";

            message += $"\nMessage: {ex.Message}\nStackTrace: {ex.StackTrace}";

            if (ex.InnerException != null)
                message += $"\nInner: {ex.InnerException.Message}";

            Logger.Error(message, ex);
        }

        public static void Info(string msg) => Logger.Info(msg);
        public static void Warn(string msg) => Logger.Warn(msg);
        public static void Debug(string msg) => Logger.Debug(msg);
    }
}
