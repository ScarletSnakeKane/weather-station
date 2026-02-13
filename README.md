Weather Station – Cloud‑Native .NET Microservice
A lightweight, containerized .NET 8 microservice that aggregates weather forecasts from multiple external providers.
The service exposes a single HTTP endpoint and returns a unified forecast response for a given city, country, and date.
It is deployed to Azure Container Apps with a full CI/CD pipeline via GitHub Actions.

NET 8 isolated Azure Function running inside a Docker container
Single REST endpoint: /api/weather
Integrates with 3 external weather providers:
  Open‑Meteo
  Visual Crossing
  WeatherAPI
  
In‑memory caching to avoid redundant external calls
Clean architecture:
  Core – domain models and interfaces
  Infrastructure – providers, DTOs, caching
  Weather.API – Azure Function host
  Unit tests for providers and aggregator
  CI/CD using GitHub Actions
  Deployed to Azure Container Apps
  Environment‑based configuration for API keys

  src/
 ├── Core/
 │    ├── Models/
 │    └── Services/
 ├── Infrastructure/
 │    ├── Providers/
 │    ├── Services/
 │    └── Dto/
 └── Weather.API/
      ├── Program.cs
      └── WeatherFunction.cs

Running Locally
  docker build -t weather-station .
  docker run -p 8080:80 \
    -e WEATHER_API_KEY=your_key \
    -e VISUAL_CROSSING_KEY=your_key \
    weather-station
    Then call: http://localhost:8080/api/weather?city=London&country=UK&date=2026-02-13
