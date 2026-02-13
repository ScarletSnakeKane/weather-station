using System.Net.Http.Json;
using Core.Models;
using Core.Services;
using Infrastructure.Dto.WeatherApi;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Providers
{
    public class WeatherApiProvider : IWeatherProvider
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public string Name => "WeatherAPI";

        public WeatherApiProvider(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config["WEATHER_API_KEY"] ?? throw new Exception("WeatherAPI key missing");
        }

        public async Task<ProviderResult> GetForecastAsync(string city, string country, DateTime date)
        {
            try
            {
                var url =
                    $"https://api.weatherapi.com/v1/forecast.json?key={_apiKey}&q={city},{country}&days=7&aqi=no&alerts=no";

                var data = await _http.GetFromJsonAsync<WeatherApiResponse>(url);

                if (data?.Forecast?.Forecastday == null)
                    return new ProviderResult { Success = false, Error = "No forecast" };

                var day = data.Forecast.Forecastday
                    .FirstOrDefault(f => f.Date == date.Date);

                if (day == null)
                    return new ProviderResult { Success = false, Error = "Date not found" };

                var forecast = new WeatherForecast
                {
                    Provider = Name,
                    Date = date,
                    TemperatureC = day.Day.AvgTempC,
                    Description = day.Day.Condition.Text
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
