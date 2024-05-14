using DCXAirApi.Infrastructure.DbContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DCXAirApi.Infrastructure.extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            using (var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
            {
                try
                {
                    //appContext.Database.Migrate();//when migrations are created
                    appContext.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
                    logger.LogError(ex, "An error occurred configuring the DB.");
                }
            }

            return host;
        }
    }
}
