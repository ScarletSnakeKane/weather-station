using Core.Models;
using Core.Services;
using Moq;
using Xunit;

public class WeatherAggregatorTests
{
    [Fact]
    public async Task GetForecastAsync_AggregatesSuccessfulProviders()
    {
        // Arrange
        var date = new DateTime(2025, 2, 10);

        var provider1 = new Mock<IWeatherProvider>();
        provider1
            .Setup(p => p.GetForecastAsync("London", "UK", date))
            .ReturnsAsync(new ProviderResult
            {
                Success = true,
                Forecast = new WeatherForecast
                {
                    Provider = "P1",
                    Date = date,
                    TemperatureC = 10,
                    Description = "Sunny"
                }
            });

        var provider2 = new Mock<IWeatherProvider>();
        provider2
            .Setup(p => p.GetForecastAsync("London", "UK", date))
            .ReturnsAsync(new ProviderResult
            {
                Success = true,
                Forecast = new WeatherForecast
                {
                    Provider = "P2",
                    Date = date,
                    TemperatureC = 12,
                    Description = "Cloudy"
                }
            });

        var provider3 = new Mock<IWeatherProvider>();
        provider3
            .Setup(p => p.GetForecastAsync("London", "UK", date))
            .ReturnsAsync(new ProviderResult
            {
                Success = false,
                Error = "API error"
            });

        var cache = new Mock<ICacheService>();
        cache
            .Setup(c => c.GetAsync<WeatherResponse>(It.IsAny<string>()))
            .ReturnsAsync((WeatherResponse?)null);

        var aggregator = new WeatherAggregator(
            new[] { provider1.Object, provider2.Object, provider3.Object },
            cache.Object
        );

        // Act
        var result = await aggregator.GetForecastAsync("London", "UK", date);

        // Assert
        Assert.Equal(2, result.Forecasts.Count);
        Assert.Contains(result.Forecasts, f => f.Provider == "P1");
        Assert.Contains(result.Forecasts, f => f.Provider == "P2");

        cache.Verify(c => c.SetAsync(
            It.IsAny<string>(),
            It.IsAny<WeatherResponse>(),
            It.IsAny<TimeSpan>()),
            Times.Once);
    }
}
