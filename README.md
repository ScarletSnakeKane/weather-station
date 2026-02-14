# Weather Station — Cloud‑Native .NET 8 Microservice
A containerized .NET 8 Azure Functions (Isolated Worker) microservice that aggregates weather forecasts from three external providers and returns a unified response.
Deployed to Azure Container Apps with full CI/CD via GitHub Actions.

## Features
- .NET 8 isolated Azure Functions
- Single REST endpoint: /api/weather
- Integrates with:
- - Open‑Meteo
- - WeatherAPI
- - Visual Crossing
- In‑memory caching
- Clean architecture (Core / Infrastructure / API)
- Unit tests for providers and aggregator
- Dockerized and deployed to Azure Container Apps
- CI/CD via GitHub Actions + Azure Container Registry
Secrets stored in Container Apps Environment Variables

## Example Requests
```text
- http://localhost:8080/api/weather?city=London&country=UK&date=2026-02-12
- https://weather-station-api.livelysmoke-0269b5a2.westeurope.azurecontainerapps.io/api/weather?city=London&country=UK&date=2026-02-13
```

## Example Response
```json
 {
  "City": "London",
  "Country": "UK",
  "Date": "2026-02-13T00:00:00",
  "Forecasts": [
    {
      "Provider": "OpenMeteo",
      "Date": "2026-02-13T00:00:00",
      "TemperatureC": 9.1,
      "Description": "Max temperature"
    },
    {
      "Provider": "WeatherAPI",
      "Date": "2026-02-13T00:00:00",
      "TemperatureC": 5.8,
      "Description": "Patchy rain nearby"
    },
    {
      "Provider": "VisualCrossing",
      "Date": "2026-02-13T00:00:00",
      "TemperatureC": 5.9,
      "Description": "Snow, Rain, Overcast"
    }
  ]
}
```

## Configuration (Environment Variables)
| Variable              | Description                     |
|-----------------------|---------------------------------|
| WEATHER_API_KEY       | API key for WeatherAPI          |
| VISUAL_CROSSING_KEY   | API key for Visual Crossing     |

## Running Locally (Docker)
- ### Create .env file
```env
WEATHER_API_KEY=your_key
VISUAL_CROSSING_KEY=your_key
```
- ### Run with Docker Compose
 ```bash
docker-compose up --build
```
- ### Test locally
 ```text
http://localhost:8080/api/weather?city=London&country=UK&date=2026-02-13
```


