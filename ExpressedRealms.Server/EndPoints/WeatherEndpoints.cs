namespace ExpressedRealms.Server.EndPoints;

internal static class WeatherEndpoints
{
    internal static void AddWeatherEndpoints(this WebApplication app)
    {
        var summaries = new[]
        {
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching"
        };

        app.MapGet(
                "/weatherforecast",
                () =>
                {
                    var forecast = Enumerable
                        .Range(1, 100)
                        .Select(index => new WeatherForecast(
                            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            Random.Shared.Next(-20, 55),
                            summaries[Random.Shared.Next(summaries.Length)]
                        ))
                        .ToArray();
                    return forecast;
                }
            )
            .WithName("GetWeatherForecast")
            .WithOpenApi();
    }

    private record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
