using MassTransit;

namespace ServerCheck.Consumers;

public class CheckGosiConsumerDefinition : ConsumerDefinition<CheckGosiConsumer>
{
    public CheckGosiConsumerDefinition()
    {
        ConcurrentMessageLimit = 2;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<CheckGosiConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Interval(3, 1000));
        endpointConfigurator.UseInMemoryOutbox(context);
        endpointConfigurator.PrefetchCount = 16;

        base.ConfigureConsumer(endpointConfigurator, consumerConfigurator, context);
    }
}