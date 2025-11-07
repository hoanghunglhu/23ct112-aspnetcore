using NLog;
using System;

namespace MyApp.Helpers
{
    public static class LoggerHelper
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        public static void LogError(Exception ex)
        {
            logger.Error(ex, ex.Message);
        }

        public static void LogInfo(string message)
        {
            logger.Info(message);
        }
    }
}
