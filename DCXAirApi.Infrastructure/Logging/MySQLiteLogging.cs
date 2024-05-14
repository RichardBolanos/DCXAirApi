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

    public class MySQLiteLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new MySQLiteLogger();
        }

        public void Dispose() { }
    }

    public class MySQLiteLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

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
