using Microsoft.AspNetCore.Mvc;
using Orleans;
using Sensify.Decoders.Common;
using Sensify.Decoders.Elsys;
using Sensify.Grains;
using Sensify.Grains.Sensors.Common;
using Sensify.Persistence;
using Sensify.Workers.Wanesy;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add Orleans
{
    builder.Host.UseOrleans(static siloBuilder =>
    {
        siloBuilder.UseMongoDBClient(siloBuilder.Configuration["MONGO_CONNECTION_STRING"]);
        siloBuilder.UseLocalhostClustering();
        siloBuilder.AddMongoDBGrainStorageAsDefault(options =>
        {
            options.DatabaseName = "sensify";
        });

        siloBuilder.AddMongoDBGrainStorage("sensorInfo", options =>
        {
            options.DatabaseName = "sensify";
        });
    });
}

builder.Services.AddHttpClient();

builder.Services.AddSingleton<IMongoPersistenceProvider>(services => new MongoPersistenceProvider(builder.Configuration["MONGO_CONNECTION_STRING"]!));

builder.Services.Configure<WanesyCredentials>(options =>
{
    options.Username = builder.Configuration["WANESY_USERNAME"]!;
    options.Password = builder.Configuration["WANESY_PASSWORD"]!;
});
builder.Services.AddHostedService<WanesySensorDataBackgroundWorker>();

const string MyAllowSpecificOrigins = "test";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://localhost:3000")
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        }
    );
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.SerializerOptions.Converters.Add(new JsonConverterUnixDateTime());
});

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);

var api = app.MapGroup("/api");

api.MapGet("/", () => DateTimeOffset.UtcNow);
api.MapPut("/sensors/{sensorId:required}", async (IGrainFactory grainFactory, [FromRoute(Name = "sensorId")] string sensorId,[FromBody] UpdateSensorInfo sensor) =>
{
    if (!SensorId.IsValid(sensorId))
    {
        return Results.BadRequest("Invalid id, id should be in the format; {sensorType:int}:{sensor id:string}");
    }
    await grainFactory.GetGrain<ISensor>(sensorId).UpdateSensorInfoAsync(sensor);
    return Results.Ok();

});

api.MapGet("/sensors/{sensorId:required}", async (IGrainFactory grainFactory, [FromRoute(Name = "sensorId")] string sensorId) =>
{
    if (!SensorId.IsValid(sensorId))
    {
        return Results.BadRequest("Invalid id, id should be in the format; {sensorType:int}:{sensor id:string}");
    }

    var sensorIndo = await grainFactory.GetGrain<ISensor>(sensorId).GetSensorInfoAsync();
    return Results.Ok(sensorIndo);

});

api.MapGet("/sensors", async (IGrainFactory grainFactory) =>
{
    var sensorIndo = await grainFactory.GetGrain<ISensorManagerGrain>(Guid.Empty).GetSensors();
    return Results.Ok(sensorIndo);

});

api.MapGet("/stream", async (
    //[FromQuery(Name = "sensorId")] string sensorId,
    //ulong startDate,
    //ulong endDate,
    IGrainFactory grainFactory,
    HttpRequest request) =>
{

    var sensorInfos = await grainFactory.GetGrain<ISensorManagerGrain>(Guid.Empty).GetSensors();
    //Random random = new();

    List<object> results = [];

    foreach (var sensorInfo in sensorInfos)
    {

        if(sensorInfo.Id is { Id: null} or not { SensorType: SupportedSensorType.Elsys})
        {
            continue;
        }

        var sensor = grainFactory.GetGrain<ISensor>(sensorInfo.Id.Value.ToString());

        await foreach( var m in sensor.GetMeasurementsAsync())
        {
            results.Add(m);
        }
    }

    return Results.Ok(results);

});

api.MapPost("/record/{sensorId:required}", async (ILoggerFactory loggerFactory, IGrainFactory grainFactory, HttpRequest request, [FromRoute(Name= "sensorId")] string sensorId /*[FromBody] Dictionary<string,object> body*/) =>
{
    //var logger = loggerFactory.CreateLogger("record");

    if (!SensorId.IsValid(sensorId))
    {
        return Results.BadRequest("Invalid id, id should be in the format; {sensorType:int}:{sensor id:string}");
    }

    var body = await new StreamReader(request.Body).ReadToEndAsync();
    await grainFactory.GetGrain<ISensor>(sensorId).UpdateMeasurementAsync(new(body));

    return Results.Ok();
});

api.MapGet("/time", (ILoggerFactory loggerFactory) => {
    var logger = loggerFactory.CreateLogger("time");

    var utcNow = DateTimeOffset.UtcNow;

    logger.LogInformation("time: {@time}", utcNow);

    return Results.Ok(utcNow.ToUnixTimeMilliseconds());
});

app.Run();

record AddSensorRequest(string SensorName, string? PayloadDecoder);
