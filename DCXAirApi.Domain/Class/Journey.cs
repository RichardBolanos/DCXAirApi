using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace DCXAirApi.Domain
{
    [SwaggerSchema(Title = "Journey", Description = "Represents a journey with flights")]
    public class Journey
    {
        [Key]
        public int JourneyId { get; set; }
        public List<Flight> Flights { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }
    }
}
