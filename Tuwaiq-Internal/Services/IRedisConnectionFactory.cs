using StackExchange.Redis;

namespace WebUI.Services;

public interface IRedisConnectionFactory
{
    ConnectionMultiplexer Connection { get; }
}