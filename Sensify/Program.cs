using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var api = app.MapGroup("/api");

api.MapPost("/record", async (ILoggerFactory loggerFactory, HttpRequest request /*[FromBody] Dictionary<string,object> body*/) =>
{
   var logger = loggerFactory.CreateLogger("record");

    logger.LogInformation("received data");

    logger.LogInformation("ContentType: {@ContentType}", request.ContentType);

    var body = await new StreamReader(request.Body).ReadToEndAsync();

    logger.LogInformation("Body: {@Body}", body);

    return Results.Ok();
});

api.MapGet("/time", (ILoggerFactory loggerFactory) => {
    var logger = loggerFactory.CreateLogger("time");

    var utcNow = DateTimeOffset.UtcNow;

    logger.LogInformation("timw: {@time}", utcNow);

    return Results.Ok(utcNow.ToUnixTimeMilliseconds());
});

app.Run();
