using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var api = app.MapGroup("/api");

api.MapPost("/record", (ILoggerFactory loggerFactory, HttpRequest request, [FromBody] Dictionary<string,object> body) =>
{
   var logger = loggerFactory.CreateLogger("test");

    logger.LogInformation("received data");

    logger.LogInformation("ContentType: {@ContentType}", request.ContentType);

    logger.LogInformation("Body: {@Body}", body);

    return Results.Ok();
});

app.Run();
