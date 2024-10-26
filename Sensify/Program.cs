using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sensify.Decoders.Common;
using Sensify.Extensions;
using Sensify.Grains;
using Sensify.Grains.Sensors.Common;
using Sensify.Persistence;
using Sensify.Workers.Wanesy;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
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

const string MyAllowSpecificOrigins = "ALL";

var _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
{
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};
_jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
_jsonSerializerOptions.Converters.Add(new JsonConverterUnixDateTime());

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
    [FromQuery(Name = "sensorId")] string sensorId,
    IGrainFactory grainFactory,
    ILogger<SensorDataStream> logger,
    HttpContext httpContext,
    CancellationToken cancellationToken) =>
{

    var response = httpContext.Response;
    var bodyWriter = response.BodyWriter;
    if (!SensorId.IsValid(sensorId))
    {
        response.StatusCode = 400;
        await bodyWriter.WriteAsync(Encoding.UTF8.GetBytes($"SensorId {sensorId}"), cancellationToken);
        return;
    }

    response.StatusCode = 200;
    response.Headers.ContentType = "text/event-stream";
    response.Headers.CacheControl = "no-store";

    await new SensorDataStream(sensorId, grainFactory, logger).StreamAsync(bodyWriter, _jsonSerializerOptions, cancellationToken);

});

api.MapPost("/record/{sensorId:required}", async (
    IGrainFactory grainFactory,
    HttpRequest request,
    [FromRoute(Name= "sensorId")] string sensorId) =>
{

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

sealed class SensorDataStream(string sensorId, IGrainFactory grainFactory, ILogger<SensorDataStream> logger)
{
    private readonly StringBuilder _resultBuilder = new();

    public async Task StreamAsync(PipeWriter writer, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var sensor = grainFactory.GetGrain<ISensor>(sensorId);

            var measurementsSource = sensor.GetMeasurementsAsync(cancellationToken: cancellationToken);
            var metricsSource = sensor.GetMetricsAsync(cancellationToken);

            var source = measurementsSource.Merge(metricsSource, cancellationToken);

            await foreach (var measurement in source)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (measurement is not ISensorMeasurement sensorMeasurement) continue;

                Write(sensorMeasurement, options);
                await writer.WriteAsync(Encoding.UTF8.GetBytes(_resultBuilder.ToString()), cancellationToken);
                await writer.FlushAsync(cancellationToken);

                _resultBuilder.Clear();

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

            }

            if (!cancellationToken.IsCancellationRequested)
                await writer.CompleteAsync();
        }catch (Exception ex) when (ex is OperationCanceledException)
        {
            logger.LogError(ex, "Operation cancelled");
        }
    }

    public void Write(ISensorMeasurement data, JsonSerializerOptions? options = null)
    {
        _resultBuilder.Append("id:").AppendLine(new DateTimeOffset(data.Timestamp).ToUnixTimeMilliseconds().ToString());

        if (data.Measurement is IMetric)
        {
            _resultBuilder.Append("event:metric:").AppendLine(sensorId);
        }
        else
        {
            _resultBuilder.Append("event:measurement:").AppendLine(sensorId);
        }

        _resultBuilder.Append("data:").AppendLine(JsonSerializer.Serialize(data, options))
            .AppendLine();
    }
}
