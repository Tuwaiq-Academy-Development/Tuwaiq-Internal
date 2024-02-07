using StackExchange.Redis;

namespace WebUI.Services;

public class RedisConnectionFactory : IRedisConnectionFactory
{
    private readonly Lazy<ConnectionMultiplexer> _connection;

    public RedisConnectionFactory(string connectionString)
    {
        _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString));
    }

    public ConnectionMultiplexer Connection => _connection.Value;
}