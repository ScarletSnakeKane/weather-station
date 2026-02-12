using Core.Models;

namespace Core.Services
{
    public class WeatherAggregator : IWeatherAggregator
    {
        private readonly IEnumerable<IWeatherProvider> _providers;
        private readonly ICacheService _cache;

        public WeatherAggregator(IEnumerable<IWeatherProvider> providers, ICacheService cache)
        {
            _providers = providers;
            _cache = cache;
        }

        public async Task<WeatherResponse> GetForecastAsync(string city, string country, DateTime date)
        {
            var cacheKey = $"weather:{city}:{country}:{date:yyyy-MM-dd}";

            var cached = await _cache.GetAsync<WeatherResponse>(cacheKey);
            if (cached != null)
                return cached;

            var tasks = _providers.Select(p => p.GetForecastAsync(city, country, date));
            var results = await Task.WhenAll(tasks);

            var forecasts = results
                .Where(r => r.Success && r.Forecast != null)
                .Select(r => r.Forecast!)
                .ToList();

            var response = new WeatherResponse
            {
                City = city,
                Country = country,
                Date = date,
                Forecasts = forecasts
            };

            await _cache.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30));

            return response;
        }
    }
}

