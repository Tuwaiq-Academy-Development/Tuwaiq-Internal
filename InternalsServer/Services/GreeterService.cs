using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace InternalsServer.Services;

public class ConnectionManager
{
    private readonly Dictionary<string, IServerStreamWriter<PingReply>> _connections = new();

    public void AddConnection(string clientId, IServerStreamWriter<PingReply> responseStream)
    {
        _connections[clientId] = responseStream;
    }

    public void RemoveConnection(string clientId)
    {
        _connections.Remove(clientId);
    }

    public IServerStreamWriter<PingReply> GetConnection(string clientId)
    {
        return _connections.TryGetValue(clientId, out var stream) ? stream : null;
    }
}


public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    private readonly ConnectionManager _connectionManager;

    public GreeterService(ILogger<GreeterService> logger,ConnectionManager connectionManager)
    {
        _logger = logger;
        _connectionManager = connectionManager;
    }

    // public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    // {
    //     return Task.FromResult(new HelloReply
    //     {
    //         Message = "hey " + request.Name
    //     });
    // }

    public override async Task Ping(PingRequest request, IServerStreamWriter<PingReply> responseStream, ServerCallContext context)
    {
        _connectionManager.AddConnection(request.ClientId, responseStream);
        var i = 0;
        while (!context.CancellationToken.IsCancellationRequested)
        {
            await responseStream.WriteAsync(new PingReply
            {
                Message = $"Ping {request.ClientId} {i++}"
            });
            await Task.Delay(1000);
        }
    }
}