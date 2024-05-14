using DCXAirApi.Application.Interfaces;
using DCXAirApi.Application.Services;
using DCXAirApi.Domain.Dto;
using DCXAirApi.Infrastructure.DCXAirDbContext;
using DCXAirApi.Infrastructure.Loaders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

    public void ConfigureServices(IServiceCollection services)
    {
        AddDatabase(services);
        AddControllersAndSwagger(services);
        AddScopedServices(services);
        AddHttpClient(services);
        AddCorsPolicy(services);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DCXAir API V1");
                c.RoutePrefix = "swagger";
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowSpecificOrigin");
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        MigrateDatabase(app);
    }

    private void AddDatabase(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(_configuration.GetConnectionString("DefaultConnection")));
    }

    private void AddControllersAndSwagger(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "DCXAir API", Version = "v1" });
        });
    }

    private void AddHttpClient(IServiceCollection services) {
        // Configuración de HttpClient
        services.AddHttpClient<CurrencyConversionService>();

        // Configuración de la API de Currency Converter
        services.Configure<CurrencyConverterApiOptions>(_configuration.GetSection("CurrencyConverterApi"));
    }

    private void AddScopedServices(IServiceCollection services)
    {
        services.AddScoped<IFlightService, FlightService>();
        services.AddScoped<JsonFlightLoaderService>();

    }

    private void AddCorsPolicy(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }

    private void MigrateDatabase(IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
