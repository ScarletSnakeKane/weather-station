using System.Net;
using System.Net.Http;
using System.Text;
using Infrastructure.Providers;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Xunit;

public class WeatherApiProviderTests
{
    [Fact]
    public async Task GetForecastAsync_ReturnsSuccess_WhenDataIsValid()
    {
        // Arrange
        var json = """
        {
          "forecast": {
            "forecastday": [
              {
                "date": "2025-02-10",
                "day": {
                  "avgtemp_c": 5.0,
                  "condition": { "text": "Cloudy" }
                }
              }
            ]
          }
        }
        """;

        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri!.AbsoluteUri.Contains("api.weatherapi.com")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

        var httpClient = new HttpClient(handlerMock.Object);

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["WeatherApi:Key"] = "test-key"
            })
            .Build();

        var provider = new WeatherApiProvider(httpClient, config);

        var date = new DateTime(2025, 2, 10);

        // Act
        var result = await provider.GetForecastAsync("London", "UK", date);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Forecast);
        Assert.Equal("WeatherAPI", result.Forecast.Provider);
        Assert.Equal(5.0, result.Forecast.TemperatureC);
        Assert.Equal("Cloudy", result.Forecast.Description);
        Assert.Equal(date, result.Forecast.Date);

        // Проверяем, что запрос был отправлен
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri!.AbsoluteUri.Contains("api.weatherapi.com")),
            ItExpr.IsAny<CancellationToken>()
        );
    }
}
