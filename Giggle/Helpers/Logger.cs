using Microsoft.Extensions.Logging;
using System;

namespace Giggle.Helpers
{
    public class Logger<T> where T : class
    {
        private readonly ILogger<T> _logger;

        public Logger(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogError(string message, Exception ex = null)
        {
            if (ex == null)
            {
                _logger.LogError(message);
            }
            else
            {
                _logger.LogError(ex, message);
            }
        }
    }
}
