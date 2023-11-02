using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;

var builder = WebApplication.CreateBuilder(args);

// Adicionar os serviços ao container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();

// Obter o serviço de logging.
var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("EventGridHookLogger");

// Configure o pipeline de requisições HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();

app.MapPost("/eventgridhook", (List<EventGridEvent> events) =>
{
    foreach (var eventGridEvent in events)
    {
        if (eventGridEvent.EventType == "Microsoft.Storage.BlobCreated" && eventGridEvent.Data.ContentLength > 0)
        {
            logger.LogInformation($"Blob created with URL: {eventGridEvent.Data.Url}");
        }
    }
    return Results.Ok(new { response = "Event processed successfully." });
})
.WithName("EventGridHook");

app.Run();

public class EventGridData
{
    public string Api { get; set; }
    public string Url { get; set; }
    public long ContentLength { get; set; }
    // ... você pode adicionar mais propriedades conforme necessário
}

public class EventGridEvent
{
    public string EventType { get; set; }
    public string Subject { get; set; }
    public DateTime EventTime { get; set; }
    public EventGridData Data { get; set; }
    // ... você pode adicionar mais propriedades conforme necessário
}