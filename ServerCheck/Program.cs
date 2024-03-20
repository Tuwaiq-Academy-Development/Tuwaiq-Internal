using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DAL;
using DAL.Models;
using IAM.Application.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tuwaiq_ServerCheck.Settings;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options
        .UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion("8.0.33"), x =>
            x.CommandTimeout(180).EnableRetryOnFailure())
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var messageBrokerSettings = new MessageBrokerSettings();

builder.Configuration.GetSection(nameof(MessageBrokerSettings)).Bind(messageBrokerSettings);

builder.Services.AddOptions<MessageBrokerSettings>()
    .BindConfiguration(nameof(MessageBrokerSettings))
    .Validate(config =>
    {
        if (string.IsNullOrEmpty(config.Host) || config.Host.Length > 150)
            return false;
        if (string.IsNullOrEmpty(config.VirtualHost) || config.Host.Length > 150)
            return false;
        if (string.IsNullOrEmpty(config.Username) || config.Username.Length > 150)
            return false;
        if (string.IsNullOrEmpty(config.Password) || config.Password.Length > 150)
            return false;
        return true;
    })
    ;

    builder.Services.AddMassTransit(x =>
    {
        x.SetKebabCaseEndpointNameFormatter();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.ConfigureEndpoints(context, KebabCaseEndpointNameFormatter.Instance);

            cfg.UseNewtonsoftRawJsonSerializer();
            cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

            cfg.ConfigureJsonSerializerOptions(options =>
            {
                options.ReferenceHandler = ReferenceHandler.Preserve;
                options.WriteIndented = true;
                options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.Converters.Add(new JsonStringEnumConverter());
                return options;
            });

            cfg.Host(new Uri(messageBrokerSettings.Host), messageBrokerSettings.VirtualHost, h =>
            {
                h.Username(messageBrokerSettings.Username);
                h.Password(messageBrokerSettings.Password);

                h.ConfigureBatchPublish(x =>
                {
                    x.Enabled = true;
                    x.Timeout = TimeSpan.FromMicroseconds(2);
                });
            });
        });
        
        
        var entryAssembly = Assembly.GetEntryAssembly();

        x.AddConsumers(entryAssembly);

    });


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.MapGet("/check/{code}", async (string code, ApplicationDbContext dbContext) =>
    {
        var hashids = new HashidsNet.Hashids("Tuwaiq-Internal");
        var id = hashids.Decode(code);
        if (id.Length == 0)
        {
            return Results.BadRequest();
        }

        var date = id[0];
        if (date.ToString() != $"{DateTime.Now:ddMMyyyy}")
        {
            return Results.BadRequest();
        }

        var value = builder.Configuration.GetValue<int?>("NumberOfChecks");
        var checkLists = await dbContext.CheckList.OrderBy(s => s.CreatedOn).Skip(0)
            .Take(value ?? 10)
            .ToListAsync();
        
        return Results.Ok(checkLists);
    })
    .WithName("Check")
    .WithOpenApi()
    .Produces<List<CheckList>>(StatusCodes.Status200OK);

app.MapPost("/save/{code}/{nationalId}",
        async (string code, string nationalId, [FromBody] CheckLog? model, ApplicationDbContext dbContext) =>
        {
            var hashids = new HashidsNet.Hashids("Tuwaiq-Internal");
            var id = hashids.Decode(code);
            if (id.Length == 0)
            {
                return Results.BadRequest();
            }

            if (model == null)
            {
                return Results.BadRequest();
            }

            var ccc = id[0].ToString();

            if (ccc != DateTime.Now.ToString("ddMMyyyy"))
            {
                return Results.BadRequest();
            }

            var result = await dbContext.CheckList.FirstOrDefaultAsync(x => x.NationalId == nationalId && x.Id == model.Id);
            if (result == null)
            {
                Console.WriteLine($"Not Found {nationalId} - {model.Id}");
                return Results.NotFound();
            }

            try
            {
                dbContext.CheckList.Remove(result);
                var timeUtc = DateTime.UtcNow;
                var saTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Riyadh");
                model.CheckedOn = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, saTimeZone);
                await dbContext.CheckLogs.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return Results.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Results.BadRequest();
            }
        })
    .WithName("Save")
    .WithOpenApi();


app.Run();