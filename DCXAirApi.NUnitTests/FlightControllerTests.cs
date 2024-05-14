using DCXAirApi.Domain.Dto;
using Moq;
using DCXAirApi.Application.Interfaces;
using DCXAirApi.Domain.Class;
using DCXAirApi.Infrastructure.Loaders;
using Microsoft.AspNetCore.Mvc;
using DCXAirApi.Infrastructure.DCXAirDbContext;
using Microsoft.EntityFrameworkCore;
using DCXAirApi.Presentation.controllers;

namespace DCXAirApi.NUnitTests
{
    /// <summary>
    /// Test class for FlightController.
    /// </summary>
    [TestFixture]
    public class FlightControllerTests
    {
        private FlightController _flightController;
        private Mock<IFlightService> _flightServiceMock;
        private Mock<JsonFlightLoaderService> _jsonFlightLoaderServiceMock;

        [SetUp]
        public void Setup()
        {
            _flightServiceMock = new Mock<IFlightService>();

            // Configure DbContextOptions for SQLite
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Data Source=DCXAirDBTest.db")
                .Options;

            // Initialize DbContext with options
            var dbContext = new ApplicationDbContext(options);

            // Ensure the database is created
            dbContext.Database.EnsureCreated();

            _jsonFlightLoaderServiceMock = new Mock<JsonFlightLoaderService>(dbContext);
            _flightController = new FlightController(_flightServiceMock.Object, _jsonFlightLoaderServiceMock.Object);
        }

        /// <summary>
        /// Test that GetFlights returns the correct result.
        /// </summary>
        [Test]
        public async Task GetFlights_ReturnsCorrectResult()
        {
            // Arrange
            var flightRequest = new FlightRequest
            { Origin = "BOG", Destination = "PEI", Currency = "USD", OneWay = true };
            var journey = new Journey { Flights = new List<Flight>(), Price = 0 };
            var flight1 = new Flight
            {
                Origin = "BOG",
                Destination = "PEI",
                FlightId = 1,
                Price = 1000,
                Transport = new Transport
                {
                    FlightCarrier = "AV",
                    FlightNumber = "2020",
                    TransportId = 1
                }
            };
            var flight2 = new Flight
            {
                Origin = "PEI",
                Destination = "BOG",
                FlightId = 1,
                Price = 1500,
                Transport = new Transport
                {
                    FlightCarrier = "AV",
                    FlightNumber = "3030",
                    TransportId = 1
                }
            };
            journey.Flights = new List<Flight> { flight1, flight2 };
            journey.Origin = "BOG";
            journey.Destination = "PEI";
            journey.Price = flight1.Price + flight2.Price;
            journey.JourneyId = 1;

            _flightServiceMock.Setup(service => service.GetFlights(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(journey);

            // Act
            var result = await _flightController.GetFlights(flightRequest);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(okResult.Value);
        }

        /// <summary>
        /// Test that GetCountries returns the correct result.
        /// </summary>
        [Test]
        public async Task GetCountries_ReturnsCorrectResult()
        {
            var countries = new List<string> { "Country1", "Country2", "Country3" }; // your mocked countries list
            _flightServiceMock.Setup(service => service.GetCountries()).ReturnsAsync(countries);

            // Act
            var result = await _flightController.GetCountries();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(okResult.Value);
        }

        /// <summary>
        /// Test that LoadJson returns the correct result.
        /// </summary>
        [Test]
        public void LoadJson_ReturnsCorrectResult()
        {
            // Act
            var result = _flightController.LoadJson();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(okResult.Value);
        }
    }
}
