var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

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
                .Range(1, 5)
                .Select(
                    index =>
                        new WeatherForecast(
                            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            Random.Shared.Next(-20, 55),
                            summaries[Random.Shared.Next(summaries.Length)]
                        )
                )
                .ToArray();
            return forecast;
        }
    )
    .WithName("GetWeatherForecast");

app.MapGet(
        "/random/numbers",
        (int? count = 5, int? min = 1, int? max = 100) =>
        {
            var random = Random.Shared;
            var numbers = Enumerable
                .Range(0, count ?? 5)
                .Select(_ => random.Next(min ?? 1, (max ?? 100) + 1))
                .ToArray();
            
            var average = numbers.Average();
            
            return new
            {
                Count = count ?? 5,
                Min = min ?? 1,
                Max = max ?? 100,
                Average = Math.Round(average, 2),
                Numbers = numbers,
                GeneratedAt = DateTime.Now
            };
        }
    )
    .WithName("GetRandomNumbers");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

// prueba luisa