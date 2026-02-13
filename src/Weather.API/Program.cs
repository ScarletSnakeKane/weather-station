using Core.Services;
using Infrastructure.Services;
using Infrastructure.Providers;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

builder.Services.AddHttpClient<OpenMeteoProvider>();
builder.Services.AddHttpClient<WeatherApiProvider>();
builder.Services.AddHttpClient<VisualCrossingProvider>();

builder.Services.AddTransient<IWeatherProvider, OpenMeteoProvider>();
builder.Services.AddTransient<IWeatherProvider, WeatherApiProvider>();
builder.Services.AddTransient<IWeatherProvider, VisualCrossingProvider>();

builder.Services.AddSingleton<IWeatherAggregator, WeatherAggregator>();

var app = builder.Build();
app.Run();
