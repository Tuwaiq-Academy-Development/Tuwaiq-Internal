using DAL;
using DAL.Enums;
using DAL.Models;
using IAM.Application.Consumers;
using MassTransit;

namespace ServerCheck.Consumers;

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