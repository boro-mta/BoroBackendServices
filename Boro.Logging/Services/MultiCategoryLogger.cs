using Microsoft.Extensions.Logging;

namespace Boro.Logging.Services
{
    internal class MultiCategoryLogger<T> : ILogger<T> where T : class
    {
        private readonly ILogger<T> _logger;

        public MultiCategoryLogger(ILogger<T> logger)
        {
            _logger = logger;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return _logger.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _logger.Log(logLevel, eventId, state, exception, formatter);
        }
    }
}
