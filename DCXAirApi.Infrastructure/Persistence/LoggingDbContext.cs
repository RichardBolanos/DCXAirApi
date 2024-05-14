using DCXAirApi.Domain.Class;
using Microsoft.EntityFrameworkCore;

namespace DCXAirApi.Infrastructure.DCXAirDbContext
{
    /// <summary>
    /// Database context for logging purposes.
    /// </summary>
    public class LoggingDbContext : DbContext
    {
        /// <summary>
        /// Collection of log entries.
        /// </summary>
        public DbSet<LogEntry> Logs { get; set; }

        /// <summary>
        /// Configures the database connection options.
        /// </summary>
        /// <param name="optionsBuilder">The options for configuring the context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=DCXAirDB.db");
        }

        /// <summary>
        /// Configures the model for the database.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for the context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Timestamp).IsRequired();
                entity.Property(e => e.LogLevel).IsRequired();
                entity.Property(e => e.Message).IsRequired();
            });
        }
    }
}
