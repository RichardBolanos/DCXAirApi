using Microsoft.AspNetCore.Mvc;
using DCXAirApi.Application;
using DCXAirApi.Domain;
using Swashbuckle.AspNetCore.Annotations;

namespace DCXAirApi.Presentation.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet("one-way")]
        [SwaggerOperation(Summary = "Get one-way flights", Description = "Get one-way flights from origin to destination.")]
        [SwaggerResponse(200, "Returns the list of one-way flights", typeof(Journey))]
        public async Task<IActionResult> GetOneWayFlights(string origin, string destination, string currency)
        {
            var journey = await _flightService.GetOneWayFlights(origin, destination, currency);
            if (journey == null)
            {
                return NotFound();
            }
            return Ok(journey);
        }
    }
}
