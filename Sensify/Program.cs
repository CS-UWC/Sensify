using System.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Orleans.Hosting;
using Sensify.Grains;
using Sensify.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add Orleans
{
    builder.Host.UseOrleans(static siloBuilder =>
    {
        siloBuilder.UseMongoDBClient(siloBuilder.Configuration["MONGO_CONNECTION_STRING"]);
        siloBuilder.UseLocalhostClustering();
        siloBuilder.AddMongoDBGrainStorage("sensorInfo", options =>
        {
            options.DatabaseName = "sensify";
        });
    });
}

builder.Services.AddSingleton<IMongoPersistenceProvider>(services => new MongoPersistenceProvider(builder.Configuration["MONGO_CONNECTION_STRING"]!));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var api = app.MapGroup("/api");

api.MapGet("/", () => DateTimeOffset.UtcNow);

api.MapPost("/record/{sensorId:required}", async (ILoggerFactory loggerFactory, IGrainFactory grainFactory, HttpRequest request, [FromRoute(Name= "sensorId")] string sensorId /*[FromBody] Dictionary<string,object> body*/) =>
{
   //var logger = loggerFactory.CreateLogger("record");

    var body = await new StreamReader(request.Body).ReadToEndAsync();
    await grainFactory.GetGrain<ISensor>(sensorId).UpdateMeasurementAsync(body);

    return Results.Ok();
});

api.MapGet("/time", (ILoggerFactory loggerFactory) => {
    var logger = loggerFactory.CreateLogger("time");

    var utcNow = DateTimeOffset.UtcNow;

    logger.LogInformation("timw: {@time}", utcNow);

    return Results.Ok(utcNow.ToUnixTimeMilliseconds());
});

app.Run();
