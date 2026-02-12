using System.Text.Json.Serialization;

public record DailyData([property: JsonPropertyName("temperature_2m_max")] List<double> TemperatureMax);

