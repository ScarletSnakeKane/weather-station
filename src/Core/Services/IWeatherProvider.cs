using Core.Models;

namespace Core.Services
{
    public interface IWeatherProvider
    {
        string Name { get; }
        Task<ProviderResult> GetForecastAsync(string city, string country, DateTime date);
    }
}
