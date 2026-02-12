namespace Core.Models
{
    public class WeatherResponse
    {
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public List<WeatherForecast> Forecasts { get; set; } = new();
    }
}
