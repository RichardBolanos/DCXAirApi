using DCXAirApi.Domain;

namespace DCXAirApi.Application
{
    public interface IFlightService
    {
        Journey GetFlights(string origin, string destination, string currency);
        Task<List<String>> GetCountries();
        // Otros métodos para manejar otras consultas y conversiones de moneda
    }


}
