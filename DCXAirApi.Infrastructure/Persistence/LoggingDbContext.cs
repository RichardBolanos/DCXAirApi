using DCXAirApi.Domain.Class;
using Microsoft.EntityFrameworkCore;

namespace DCXAirApi.Infrastructure.DCXAirDbContext
{
    public class LoggingDbContext : DbContext
    {
        public DbSet<LogEntry> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=DCXAirDB.db");
        }

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
