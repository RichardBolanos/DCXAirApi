using DCXAirApi.Domain.Class;
using DCXAirApi.Infrastructure.DCXAirDbContext;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DCXAirApi.Infrastructure.Loaders
{
    /// <summary>
    /// Service for loading flights from a JSON file into the database.
    /// </summary>
    public class JsonFlightLoaderService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the JsonFlightLoaderService class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public JsonFlightLoaderService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Loads flights from a JSON file into the database.
        /// </summary>
        /// <param name="jsonFilePath">The path to the JSON file containing flight data.</param>
        /// <returns>The list of flights loaded from the JSON file.</returns>
        public List<Flight> LoadFlightsFromJson(string jsonFilePath)
        {
            string json = File.ReadAllText(jsonFilePath);
            var flights = JsonConvert.DeserializeObject<List<Flight>>(json);
            if (flights == null)
            {
                flights = new List<Flight>();
            }
            _context.Flights.AddRange(flights);
            _context.SaveChanges();

            return flights;
        }
    }
}
