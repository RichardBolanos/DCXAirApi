﻿using System.ComponentModel.DataAnnotations;

namespace DCXAirApi.Domain.Class
{
    public class Transport
    {
        [Key]
        public int TransportId { get; set; }
        public string FlightCarrier { get; set; }
        public string FlightNumber { get; set; }
    }
}
