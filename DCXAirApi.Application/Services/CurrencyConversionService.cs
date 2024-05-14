using DCXAirApi.Domain.Dto;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace DCXAirApi.Application.Services
{
    public class CurrencyConversionService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _apiKey;

        public CurrencyConversionService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["CurrencyConverterApi:BaseUrl"] ?? throw new ArgumentNullException("CurrencyConverterApi:BaseUrl");
            _apiKey = configuration["CurrencyConverterApi:ApiKey"] ?? throw new ArgumentNullException("CurrencyConverterApi:ApiKey");
        }

        public async Task<CurrencyConversionResponse> ConvertCurrencyAsync(string fromCurrency, string toCurrency)
        {
            string url = $"{_baseUrl}/conversion_rate?from={fromCurrency}&to={toCurrency}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("X-RapidAPI-Key", _apiKey);
            request.Headers.Add("X-RapidAPI-Host", "currency-converter241.p.rapidapi.com");

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CurrencyConversionResponse>();
            }
            else
            {
                return null;
            }
        }
    }
}
