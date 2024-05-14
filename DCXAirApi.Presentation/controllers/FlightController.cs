using System.Collections.Generic;
using System.Threading.Tasks;
using DCXAirApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using DCXAirApi.Domain.Dto;
using DCXAirApi.Domain.Class;
using DCXAirApi.Application.Interfaces;
using DCXAirApi.Infrastructure.Loaders;

namespace DCXAirApi.Presentation.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly JsonFlightLoaderService _jsonFlightLoaderService;

        public FlightController(IFlightService flightService, JsonFlightLoaderService jsonFlightLoaderService)
        {
            _flightService = flightService;
            _jsonFlightLoaderService = jsonFlightLoaderService;
        }

        /// <summary>
        /// Retrieves one-way flights based on the provided data.
        /// </summary>
        [HttpPost("get-flights")]
        [SwaggerOperation(Summary = "Get one-way flights", Description = "Get flights from origin to destination.")]
        [SwaggerResponse(200, "Returns the list of flights", typeof(ApiResponse<Journey>))]
        public async Task<IActionResult> GetFlights([FromBody] FlightRequest data)
        {
            var origin = data.Origin.ToUpper();
            var destination = data.Destination.ToUpper();
            var currency = data.Currency.ToUpper();
            var oneWay = data.OneWay;

            var journeyResult = await _flightService.GetFlights(origin, destination, currency);


            if (journeyResult == null)
            {
                return NotFound(new ApiResponse<Journey>(new Journey(), "No se encontraron vuelos."));
            }


            var graph = new Graph(journeyResult.Flights);
            var outboundFlights = graph.Dijkstra(origin, destination);

            if (outboundFlights.Count == 0)
            {
                journeyResult.Flights = outboundFlights;
                journeyResult.Price = 0;
                return NotFound(new ApiResponse<Journey>(journeyResult, "No se encontraron vuelos."));
            }

            if (!oneWay)
            {
                var returnFlights = graph.Dijkstra(destination, origin);
                if (returnFlights.Count == 0)
                {
                    journeyResult.Flights = outboundFlights;
                    return NotFound(new ApiResponse<Journey>(journeyResult, "No se encontraron vuelos de regreso."));
                }
                journeyResult.Flights = outboundFlights.Concat(returnFlights).ToList();
            }
            else
            {
                journeyResult.Flights = outboundFlights;
            }

            journeyResult.Price = journeyResult.Flights.Sum(f => f.Price);

            
            var journey = journeyResult;
            var apiResponse = new ApiResponse<Journey>(journey);
            return Ok(apiResponse);
        }

        /// <summary>
        /// Retrieves the list of countries involved in flights.
        /// </summary>
        [HttpGet("get-countries")]
        [SwaggerOperation(Summary = "Get countries of flights", Description = "Get the total of countries in the flights.")]
        [SwaggerResponse(200, "Returns the list of countries", typeof(ApiResponse<List<string>>))]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _flightService.GetCountries();
            if (countries == null || countries.Count == 0)
            {
                return NotFound(new ApiResponse<List<string>>([], "No se encontraron países."));
            }
            return Ok(new ApiResponse<List<string>>(countries));
        }

        /// <summary>
        /// Loads flights from a JSON file into the database.
        /// </summary>
        [HttpGet("load-json")]
        [SwaggerOperation(Summary = "Load flights from JSON", Description = "Load flights from a JSON file into the database.")]
        [SwaggerResponse(200, "JSON successfully loaded into the database.", typeof(ApiResponse<string>))]
        public IActionResult LoadJson()
        {
            _jsonFlightLoaderService.LoadFlightsFromJson("markets.json");
            return Ok(new ApiResponse<string>("JSON cargado exitosamente en la base de datos."));
        }
    }
}
