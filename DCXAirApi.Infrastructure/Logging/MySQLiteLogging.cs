using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCXAirApi.Infrastructure.Logging
{
    using DCXAirApi.Domain.Class;
    using DCXAirApi.Infrastructure.DCXAirDbContext;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Provider for creating instances of MySQLiteLogger.
    /// </summary>
    public class MySQLiteLoggerProvider : ILoggerProvider
    {
        /// <summary>
        /// Creates a new instance of MySQLiteLogger.
        /// </summary>
        /// <param name="categoryName">The category name for the logger.</param>
        /// <returns>A new instance of MySQLiteLogger.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new MySQLiteLogger();
        }

        /// <summary>
        /// Disposes of the logger provider.
        /// </summary>
        public void Dispose() { }
    }

    /// <summary>
    /// Logger implementation for logging to SQLite database.
    /// </summary>
    public class MySQLiteLogger : ILogger
    {
        /// <inheritdoc/>
        public IDisposable BeginScope<TState>(TState state) => null;

        /// <inheritdoc/>
        public bool IsEnabled(LogLevel logLevel) => true;

        /// <inheritdoc/>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (logLevel >= LogLevel.Error)
            {
                using (var db = new LoggingDbContext())
                {
                    db.Database.EnsureCreated();
                    db.Logs.Add(new LogEntry
                    {
                        LogLevel = logLevel.ToString(),
                        Message = formatter(state, exception),
                        Timestamp = DateTime.UtcNow
                    });
                    db.SaveChanges();
                }
            }
        }
    }
}
