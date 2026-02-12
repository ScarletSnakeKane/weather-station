namespace Core.Models
{
    public class ProviderResult
    {
        public bool Success { get; set; }
        public WeatherForecast? Forecast { get; set; }
        public string? Error { get; set; }
    }
}

