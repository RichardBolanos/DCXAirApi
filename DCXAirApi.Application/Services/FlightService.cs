using DCXAirApi.Application.Interfaces;
using DCXAirApi.Domain;
using DCXAirApi.Domain.Class;
using DCXAirApi.Infrastructure.DCXAirDbContext;
using Microsoft.EntityFrameworkCore;

namespace DCXAirApi.Application.Services
{
    /// <summary>
    /// Service for managing flight-related operations.
    /// </summary>
    public class FlightService : IFlightService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly CurrencyConversionService _currencyConversionService;

        /// <summary>
        /// Initializes a new instance of the FlightService class.
        /// </summary>
        /// <param name="dbContext">The application database context.</param>
        /// <param name="currencyConversionService">The currency conversion service.</param>
        public FlightService(ApplicationDbContext dbContext, CurrencyConversionService currencyConversionService)
        {
            _dbContext = dbContext;
            _currencyConversionService = currencyConversionService;
        }

        /// <inheritdoc/>
        public async Task<Journey> GetFlights(string origin, string destination, string currency)
        {
            // Consult one-way flights from origin to destination in the database
            var oneWayFlights = _dbContext.Flights.Include(f => f.Transport).ToList();

            // Perform currency conversion if necessary
            if (currency != "USD")
            {
                var conversionRate = await _currencyConversionService.ConvertCurrencyAsync("USD", currency);
                if (conversionRate != null)
                {
                    foreach (var flight in oneWayFlights)
                    {
                        flight.Price *= conversionRate.Rate;
                    }
                }
                else
                {
                    throw new Exception("Unable to convert to the selected currency");
                }
            }

            // Build the Journey object with the obtained flights
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

        /// <inheritdoc/>
        public async Task<List<string>> GetCountries()
        {
            var uniqueOrigins = await _dbContext.Flights.Select(f => f.Origin).Distinct().ToListAsync();
            var uniqueDestinations = await _dbContext.Flights.Select(f => f.Destination).Distinct().ToListAsync();

            // Combine unique origins and destinations and remove duplicates
            var uniqueCountries = uniqueOrigins.Concat(uniqueDestinations).Distinct().ToList();

            return uniqueCountries;
        }
    }
}
