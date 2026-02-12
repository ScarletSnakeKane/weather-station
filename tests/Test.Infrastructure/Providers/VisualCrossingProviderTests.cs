using System.Net;
using System.Net.Http;
using System.Text;
using Infrastructure.Providers;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Xunit;

public class VisualCrossingProviderTests
{
    [Fact]
    public async Task GetForecastAsync_ReturnsSuccess_WhenDataIsValid()
    {
        // Arrange
        var json = """
        {
          "days": [
            {
              "temp": 12.5,
              "conditions": "Partly Cloudy"
            }
          ]
        }
        """;

        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri!.AbsoluteUri.Contains("weather.visualcrossing.com")),
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
                ["VisualCrossing:Key"] = "test-key"
            })
            .Build();

        var provider = new VisualCrossingProvider(httpClient, config);

        var date = new DateTime(2025, 2, 10);

        // Act
        var result = await provider.GetForecastAsync("London", "UK", date);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Forecast);
        Assert.Equal("VisualCrossing", result.Forecast.Provider);
        Assert.Equal(12.5, result.Forecast.TemperatureC);
        Assert.Equal("Partly Cloudy", result.Forecast.Description);
        Assert.Equal(date, result.Forecast.Date);

        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri!.AbsoluteUri.Contains("weather.visualcrossing.com")),
            ItExpr.IsAny<CancellationToken>()
        );
    }
}

