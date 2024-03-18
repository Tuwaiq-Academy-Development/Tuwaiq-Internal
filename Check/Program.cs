using Check.BackgroundJobs;
using Check.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IGOSIEmploymentStatusService, GOSIEmploymentStatusServiceClient>();

builder.Services.AddSingleton<PeriodicHostedService>();
builder.Services.AddHostedService(
    provider => provider.GetRequiredService<PeriodicHostedService>());


builder.Services.AddScoped<CheckFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//
// app.MapGet("/{nationalId}", async (string nationalId, IGOSIEmploymentStatusService employmentStatusService) =>
// {
//     var result = await employmentStatusService.GetEmploymentStatusAsync(nationalId);
//     Console.WriteLine($"NationalId: {nationalId} - Result: {JsonConvert.SerializeObject(result)}");
//     var item = new CheckLog() { NationalId = nationalId };
//     item.Response = JsonConvert.SerializeObject(result);
//     var obj = result.Item as EmploymentStatusStructure;
//     var name = obj?.ContributorName.Item as PersonNameBodyStructure;
//                         
//     if (obj != null)
//     {
//         item.Status = obj.ContributorStatus;
//         item.IsChecked = true;
//         item.CheckedOn = DateTime.Now;
//         item.FirstName = name?.FirstName;
//         item.SecondName = name?.SecondName;
//         item.ThirdName = name?.ThirdName;
//         item.FourthName = name?.FourthName;
//         item.LastName = name?.LastName;
//     }
//     if (result == null)
//     {
//         return Results.NotFound();
//     }
//     
//     Console.WriteLine($"NationalId: {nationalId} - Result: {JsonConvert.SerializeObject(result)}");
//
//     return Results.Ok(result);
// });
// app.UseHttpsRedirection();
app.Run();
