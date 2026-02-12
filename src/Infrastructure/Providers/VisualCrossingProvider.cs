using System.Net.Http.Json;
using Core.Models;
using Core.Services;
using Infrastructure.Dto.VisualCrossing;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Providers
{
    public class VisualCrossingProvider : IWeatherProvider
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public string Name => "VisualCrossing";

        public VisualCrossingProvider(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config["VisualCrossing:Key"]
                      ?? throw new Exception("VisualCrossing key missing");
        }

        public async Task<ProviderResult> GetForecastAsync(string city, string country, DateTime date)
        {
            try
            {
                var url =
                    $"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/{city},{country}/{date:yyyy-MM-dd}?key={_apiKey}&unitGroup=metric";

                var data = await _http.GetFromJsonAsync<VisualCrossingResponse>(url);

                if (data == null || data.Days.Count == 0)
                    return new ProviderResult { Success = false, Error = "No forecast" };

                var day = data.Days.First();

                var forecast = new WeatherForecast
                {
                    Provider = Name,
                    Date = date,
                    TemperatureC = day.Temp,
                    Description = day.Conditions
                };

                return new ProviderResult { Success = true, Forecast = forecast };
            }
            catch (Exception ex)
            {
                return new ProviderResult { Success = false, Error = ex.Message };
            }
        }
    }
}
