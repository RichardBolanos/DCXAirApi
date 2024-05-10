using DCXAirApi.Application;
using DCXAirApi.Infrastructure;
using DCXAirApi.Infrastructure.extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DCXAir API", Version = "v1" });
});

builder.Services.AddScoped<IFlightService, FlightService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DCXAir API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.MapPost("/api/Flight/one-way", async (IFlightService flightService, FlightRequest data) =>
{
    var origin = data.Origin;
    var destination = data.Destination;
    var currency = data.Currency;

    // Llama al método GetOneWayFlights de tu servicio para obtener los vuelos de solo ida
    var journey = await flightService.GetOneWayFlights(origin, destination, currency);

    if (journey == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(journey);
})
.WithName("GetOneWayFlights")
.WithMetadata(new EndpointNameMetadata("GetOneWayFlights"))
.WithOpenApi();

app.Run();
app.MigrateDatabase().Run();

public class FlightRequest
{
    [Required]
    public required string Origin { get; set; }

    [Required]
    public required string Destination { get; set; }

    [Required]
    public required string Currency { get; set; }
}
