using DCXAirApi.Domain;

namespace DCXAirApi.Application
{
    public interface IFlightService
    {
        Task<Journey> GetOneWayFlights(string origin, string destination, string currency);
        // Otros métodos para manejar otras consultas y conversiones de moneda
    }
}
