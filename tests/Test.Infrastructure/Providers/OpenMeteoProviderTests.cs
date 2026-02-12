using System.Net;
using System.Net.Http;
using System.Text;
using Core.Models;
using Infrastructure.Providers;
using Moq;
using Moq.Protected;
using Xunit;

public class OpenMeteoProviderTests
{
    [Fact]
    public async Task GetForecastAsync_ReturnsSuccess_WhenDataIsValid()
    {
        // Arrange
        var fixedDate = new DateTime(2025, 2, 10);

        var geoJson = """
        {
          "results": [
            { "latitude": 51.5, "longitude": -0.12 }
          ]
        }
        """;

        var forecastJson = """
        {
          "daily": {
            "temperature_2m_max": [10.5]
          }
        }
        """;

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri!.AbsoluteUri.StartsWith("https://geocoding-api.open-meteo.com")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(geoJson, Encoding.UTF8, "application/json")
            });
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri!.AbsoluteUri.StartsWith("https://api.open-meteo.com")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(forecastJson, Encoding.UTF8, "application/json")
            });

        var httpClient = new HttpClient(handlerMock.Object);
        var provider = new OpenMeteoProvider(httpClient, () => fixedDate);

        // Act
        var result = await provider.GetForecastAsync("London", "UK", fixedDate);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Forecast);
        Assert.Equal("OpenMeteo", result.Forecast.Provider);
        Assert.Equal(10.5, result.Forecast.TemperatureC);
        Assert.Equal("Max temperature", result.Forecast.Description);
        Assert.Equal(fixedDate, result.Forecast.Date);
    }
}
