using Grpc.Core;
using Grpc.Net.Client;

namespace InternalsClient;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            try
            {
                // using var channel = GrpcChannel.ForAddress("https://localhost:7048");
                // var client = new Greeter.GreeterClient(channel);
                var grpcClient = new GrpcClient("https://localhost:7048");
                var client = await grpcClient.ConnectAsync();
                using var call = client.Ping(new PingRequest(){ClientId = "client1"});
                await foreach (var reply in call.ResponseStream.ReadAllAsync(stoppingToken))
                {
                    logger.LogInformation("Ping: {message}", reply.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}


public class GrpcClient
{
    private readonly string _address;
    private readonly int _maxRetryAttempts;
    private readonly TimeSpan _retryInterval;

    public GrpcClient(string address, int maxRetryAttempts = 20, TimeSpan? retryInterval = null)
    {
        _address = address;
        _maxRetryAttempts = maxRetryAttempts;
        _retryInterval = retryInterval ?? TimeSpan.FromSeconds(3);
    }

    public async Task<Greeter.GreeterClient> ConnectAsync()
    {
        int attempt = 0;
        while (true)
        {
            try
            {
                var channel = GrpcChannel.ForAddress(_address);
                var client = new Greeter.GreeterClient(channel);
                // Test the connection by making a simple request
                // await client.YourTestMethodAsync(new YourTestRequest());
                return client;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
            {
                attempt++;
                if (attempt >= _maxRetryAttempts)
                {
                    throw new Exception("Failed to connect to gRPC server after multiple attempts.", ex);
                }

                Console.WriteLine($"Connection failed. Retrying in {_retryInterval.TotalSeconds} seconds...");
                await Task.Delay(_retryInterval);
            }
        }
    }
}
