using DAL;
using DAL.Enums;
using DAL.Models;
using MassTransit;

namespace IAM.Application.Consumers;

public class CheckGosi
{
    public CheckGosi(string nationalId)
    {
        NationalId = nationalId;
    }

    public string NationalId { get; set; }
}

public class CheckGosiResponse
{
    public CheckGosiResponse(string nationalId, DateTime? checkedOn, string checkType, string status)
    {
        NationalId = nationalId;
        CheckedOn = checkedOn;
        CheckType = checkType;
        Status = status;
    }

    public string NationalId { get; set; }
    public DateTime? CheckedOn { get; set; }
    public string CheckType { get; set; }
    public string Status { get; set; } = null!;
}



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

public class CheckGosiConsumer : IConsumer<CheckGosi>
{
    private readonly ApplicationDbContext _dbContext;
    
    public CheckGosiConsumer(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task Consume(ConsumeContext<CheckGosi> context)
    {
        _dbContext.CheckList.Add(new CheckList
        {
            NationalId = context.Message.NationalId,
            CheckType = CheckType.Gosi,
        });
        return _dbContext.SaveChangesAsync();
    }
}