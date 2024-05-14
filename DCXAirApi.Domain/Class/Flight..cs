using System.ComponentModel.DataAnnotations;

namespace DCXAirApi.Domain.Class
{
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }
        public Transport Transport { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }
    }
}
