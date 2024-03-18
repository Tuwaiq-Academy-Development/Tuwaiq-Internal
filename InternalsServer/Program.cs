using InternalsServer;
using InternalsServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureHttpsDefaults(httpsOptions =>
    {
        httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
    });
    options.ConfigureEndpointDefaults(endpointOptions =>
    {
        endpointOptions.Protocols = HttpProtocols.Http2;
    });
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<ConnectionManager>();
builder.Services.AddGrpc();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapPost("/send/{clientId}", async (string clientId) =>
{
    var connectionManager = app.Services.GetRequiredService<ConnectionManager>();
    var connection = connectionManager.GetConnection(clientId);
    if (connection != null)
    {
        await connection.WriteAsync(new PingReply() { Message = "test" });
        return Results.Ok("Message sent");
    }
    return Results.NotFound("Client not found");
}).WithName("Send")
.WithOpenApi();

app.MapGet("/", () => "Hello World");
app.MapGrpcService<GreeterService>();
app.Run();