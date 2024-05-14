using DCXAirApi.Domain.Class;
using DCXAirApi.Infrastructure.DCXAirDbContext;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DCXAirApi.Infrastructure.Loaders
{
    public class JsonFlightLoaderService
    {
        private readonly ApplicationDbContext _context;

        public JsonFlightLoaderService(ApplicationDbContext context)
        {
            _context = context;
        }

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
