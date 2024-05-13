
using DCXAirApi.Domain;
using DCXAirApi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DCXAirApi.Application

{
    public class FlightService : IFlightService
    {
        private readonly ApplicationDbContext _dbContext;

        public FlightService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Journey GetFlights(string origin, string destination, string currency)
        {
            // Consultar vuelos de solo ida desde el origen hasta el destino en la base de datos
            var oneWayFlights = _dbContext.Flights.Include(f => f.Transport).ToList();


            // Realizar la conversión de moneda si es necesario
            if (currency != "USD")
            {
                // Lógica para convertir el precio de los vuelos a la moneda especificada
                // (Por ejemplo, podrías llamar a un servicio de conversión de moneda externo)
                // Aquí se asume que la conversión se realiza de manera síncrona para simplificar el ejemplo
                double conversionRate = 1.2; // Supongamos una tasa de conversión fija para fines de demostración
                foreach (var flight in oneWayFlights)
                {
                    flight.Price *= conversionRate;
                }
            }

            // Construir el objeto Journey con los vuelos obtenidos
            var journey = new Journey
            {
                Origin = origin,
                Destination = destination,
                Price = oneWayFlights.Sum(f => f.Price),
                Flights = oneWayFlights.Select(f => new Flight
                {
                    Origin = f.Origin,
                    Destination = f.Destination,
                    Price = f.Price,
                    Transport = f.Transport != null ? new Transport
                    {

                        FlightCarrier = f.Transport.FlightCarrier ?? "",
                        FlightNumber = f.Transport.FlightNumber ?? ""
                    } : new Transport
                    {
                        TransportId = -1,
                        FlightCarrier = "",
                        FlightNumber = ""
                    }
                }).ToList()
            };
            return journey;
        }

        public async Task<List<String>> GetCountries()
        {
            var uniqueOrigins = await _dbContext.Flights.Select(f => f.Origin).Distinct().ToListAsync();
            var uniqueDestinations = await _dbContext.Flights.Select(f => f.Destination).Distinct().ToListAsync();

            // Unir los países únicos de origen y destino y eliminar duplicados
            var uniqueCountries = uniqueOrigins.Concat(uniqueDestinations).Distinct().ToList();

            return uniqueCountries;
        }
    }
}
