using DCXAirApi.Domain.Class;

namespace DCXAirApi.Application.Interfaces
{
    public interface IFlightService
    {
        Task<Journey> GetFlights(string origin, string destination, string currency);
        Task<List<string>> GetCountries();
        // Otros métodos para manejar otras consultas y conversiones de moneda
    }


}
