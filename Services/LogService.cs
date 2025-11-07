using log4net;
using System;
using System.Text;

namespace LearnApiNetCore.Services
{
    public class Logger : ILogService
    {
        private readonly ILog _log;

        public Logger(ILog log)
        {
            _log = log;
        }

        public void Debug(string message) => _log.Debug(message);
        public void Info(string message) => _log.Info(message);
        public void Warn(string message) => _log.Warn(message);
        public void Error(string message, Exception? ex = null) => _log.Error(BuildMessage(message, ex), ex);
        public void Fatal(string message, Exception? ex = null) => _log.Fatal(BuildMessage(message, ex), ex);

        public void LogException(Exception ex, string additionalInfo = "")
        {
            if (ex == null) return;

            var fullMessage = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(additionalInfo))
                fullMessage.AppendLine($"[Context] {additionalInfo}");

            fullMessage.AppendLine(BuildFullExceptionMessage(ex));

            _log.Error(fullMessage.ToString(), ex);
        }

        private string BuildMessage(string message, Exception? ex)
        {
            return ex == null ? message : $"{message} | Exception: {ex.Message}";
        }

        private string BuildFullExceptionMessage(Exception ex, int level = 0)
        {
            var sb = new StringBuilder();
            var indent = new string(' ', level * 2);

            sb.AppendLine($"{indent}=== Exception Level {level} ===");
            sb.AppendLine($"{indent}Type: {ex.GetType().FullName}");
            sb.AppendLine($"{indent}Message: {ex.Message}");
            sb.AppendLine($"{indent}Source: {ex.Source}");
            sb.AppendLine($"{indent}StackTrace:\n{ex.StackTrace}");

            if (ex.Data != null && ex.Data.Count > 0)
            {
                sb.AppendLine($"{indent}Data:");
                foreach (var key in ex.Data.Keys)
                {
                    sb.AppendLine($"{indent}  {key}: {ex.Data[key]}");
                }
            }

            if (ex.InnerException != null)
            {
                sb.AppendLine(BuildFullExceptionMessage(ex.InnerException, level + 1));
            }

            return sb.ToString();
        }
    }
}