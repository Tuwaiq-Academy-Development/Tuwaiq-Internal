using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options
        .UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion("8.0.33"), x =>
            x.CommandTimeout(180).EnableRetryOnFailure())
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

        return Results.Ok(await dbContext.CheckList.OrderBy(s => s.NationalId).Skip(0)
            .Take(builder.Configuration.GetValue<int?>("NumberOfChecks") ?? 10)
            .ToListAsync());
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
                return Results.NotFound();
            }

            dbContext.Remove(result);
            await dbContext.CheckLogs.AddAsync(model);
            await dbContext.SaveChangesAsync();
            return Results.Ok();
        })
    .WithName("Save")
    .WithOpenApi();


app.Run();