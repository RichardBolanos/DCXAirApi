using DCXAirApi.Domain.Class;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace DCXAirApi.Infrastructure.DCXAirDbContext
{
    /// <summary>
    /// Database context for DCXAirApi application.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Collection of journeys.
        /// </summary>
        public DbSet<Journey> Journeys { get; set; }

        /// <summary>
        /// Collection of flights.
        /// </summary>
        public DbSet<Flight> Flights { get; set; }

        /// <summary>
        /// Collection of transports.
        /// </summary>
        public DbSet<Transport> Transports { get; set; }

        /// <summary>
        /// Configures the database connection options.
        /// </summary>
        /// <param name="optionsBuilder">The options for configuring the context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=DCXAirDB.db");
        }

        /// <summary>
        /// Constructor for the ApplicationDbContext.
        /// </summary>
        /// <param name="options">The options for the context.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Configures the model for the database.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for the context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigurePrimaryKey<Flight>(modelBuilder, e => e.FlightId);
            ConfigurePrimaryKey<Journey>(modelBuilder, e => e.JourneyId);
            ConfigurePrimaryKey<Transport>(modelBuilder, e => e.TransportId);
        }

        /// <summary>
        /// Configures the primary key for an entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="modelBuilder">The builder being used to construct the model for the context.</param>
        /// <param name="keyExpression">The expression specifying the primary key property.</param>
        private void ConfigurePrimaryKey<TEntity>(ModelBuilder modelBuilder, Expression<Func<TEntity, object>> keyExpression) where TEntity : class
        {
            modelBuilder.Entity<TEntity>(entity =>
            {
                entity.HasKey(keyExpression!);
            });
        }
    }
}
