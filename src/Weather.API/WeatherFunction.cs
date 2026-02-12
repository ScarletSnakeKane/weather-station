using System.Net;
using Core.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Weather.API;

public class WeatherFunction
{
    private readonly ILogger<WeatherFunction> _logger;
    private readonly IWeatherAggregator _aggregator;

    public WeatherFunction(ILogger<WeatherFunction> logger, IWeatherAggregator aggregator)
    {
        _logger = logger;
        _aggregator = aggregator;
    }

    [Function("GetWeather")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "weather")] HttpRequestData req)
    {
        var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);

        var city = query["city"];
        var country = query["country"];
        var dateStr = query["date"];

        if (string.IsNullOrWhiteSpace(city) ||
            string.IsNullOrWhiteSpace(country) ||
            !DateTime.TryParse(dateStr, out var date))
        {
            var bad = req.CreateResponse(HttpStatusCode.BadRequest);
            await bad.WriteStringAsync("Parameters required: city, country, date (yyyy-MM-dd)");
            return bad;
        }

        var result = await _aggregator.GetForecastAsync(city, country, date);

        var ok = req.CreateResponse(HttpStatusCode.OK);
        await ok.WriteAsJsonAsync(result);
        return ok;
    }
}
