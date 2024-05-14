using DCXAirApi.Domain.Class;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace DCXAirApi.Infrastructure.DCXAirDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Transport> Transports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=DCXAirDB.db");
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigurePrimaryKey<Flight>(modelBuilder, e => e.FlightId);
            ConfigurePrimaryKey<Journey>(modelBuilder, e => e.JourneyId);
            ConfigurePrimaryKey<Transport>(modelBuilder, e => e.TransportId);

        }

        private void ConfigurePrimaryKey<TEntity>(ModelBuilder modelBuilder, Expression<Func<TEntity, object>> keyExpression) where TEntity : class
        {
            modelBuilder.Entity<TEntity>(entity =>
            {
                entity.HasKey(keyExpression!);
            });
        }
    }
}
