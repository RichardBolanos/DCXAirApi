using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCXAirApi.Domain
{
    public class FlightRequest
    {
        [Required]
        public string Origin { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        public string Currency { get; set; }
        [Required]
        public bool OneWay { get; set; }
    }
}
