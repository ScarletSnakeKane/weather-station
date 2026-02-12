using Infrastructure.Dto.WeatherApi;
using System.Text.Json.Serialization;

public record DayInfo([property: JsonPropertyName("avgtemp_c")] double AvgTempC, Condition Condition);

