using StackExchange.Redis;

namespace AdminUi.Services;

public interface IRedisConnectionFactory
{
    ConnectionMultiplexer Connection { get; }
}