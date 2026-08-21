using Microsoft.Extensions.Logging;

namespace MySimpleStockProduct.Logging
{
    public class CustomLogger<T> : ICustomLogger<T>
    {
        private readonly string _className;
        private readonly ILogger<T> _logger;

        public CustomLogger(ILogger<T> logger)
        {
            _className = typeof(T).Name;
            _logger = logger;
        }

        public void LogInfo(string message, params object[] args)
        {
            _logger.LogInformation(FormatLogg("INFO", message, args));
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(FormatLogg("WARN", message, args));
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            string errorMessage = exception != null ? $"{message} | Exceção: {exception.Message}" : message;
            _logger.LogError(FormatLogg("ERROR", errorMessage, args));
        }

        private string FormatLogg(string level, string message, params object[] args)
        {
            string formattedMessage = args.Length > 0 ? string.Format(message, args) : message;
            string timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            return $"{timestamp} -> {_className} -> {level} - {formattedMessage}";
        }
    }
}