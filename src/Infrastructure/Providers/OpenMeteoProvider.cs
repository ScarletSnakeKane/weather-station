using Core.Models;
using Core.Services;
using Infrastructure.Dto.OpenMeteo;
using System.Net.Http.Json;

namespace Infrastructure.Providers
{
    public class OpenMeteoProvider : IWeatherProvider
    {
        private readonly HttpClient _http;
        private readonly Func<DateTime> _now;

        public string Name => "OpenMeteo";

        public OpenMeteoProvider(HttpClient http, Func<DateTime>? now = null)
        {
            _http = http;
            _now = now ?? (() => DateTime.UtcNow);
        }

        public async Task<ProviderResult> GetForecastAsync(string city, string country, DateTime date)
        {
            try
            {
                var geo = await _http.GetFromJsonAsync<OpenMeteoGeoResponse>(
                    $"https://geocoding-api.open-meteo.com/v1/search?name={city}&count=1");

                if (geo?.Results == null || geo.Results.Count == 0)
                    return new ProviderResult { Success = false, Error = "City not found" };

                var lat = geo.Results[0].Latitude;
                var lon = geo.Results[0].Longitude;

                var data = await _http.GetFromJsonAsync<OpenMeteoForecastResponse>(
                    $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&daily=temperature_2m_max&timezone=auto");

                if (data?.Daily == null)
                    return new ProviderResult { Success = false, Error = "No forecast" };

                var index = (date.Date - _now().Date).Days;

                if (index < 0 || index >= data.Daily.TemperatureMax.Count)
                    return new ProviderResult { Success = false, Error = "Date out of range" };

                return new ProviderResult
                {
                    Success = true,
                    Forecast = new WeatherForecast
                    {
                        Provider = Name,
                        Date = date,
                        TemperatureC = data.Daily.TemperatureMax[index],
                        Description = "Max temperature"
                    }
                };
            }
            catch (Exception ex)
            {
                return new ProviderResult { Success = false, Error = ex.Message };
            }
        }
    }
}
