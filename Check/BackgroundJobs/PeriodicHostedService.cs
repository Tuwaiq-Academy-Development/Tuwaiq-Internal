using Check.Services;
using DAL.Models;
using Newtonsoft.Json;
using RestSharp;

namespace Check.BackgroundJobs;

public class PeriodicHostedService(
    ILogger<PeriodicHostedService> logger,
    IServiceScopeFactory factory,
    CheckFactory checkFactory)
    : BackgroundService
{
    private readonly TimeSpan _period = TimeSpan.FromMinutes(1);
    private readonly int _executionCount = 0;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(_period);
        while (
            !stoppingToken.IsCancellationRequested &&
            await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await using var scope = factory.CreateAsyncScope();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var url = configuration["Url"] ?? "";

                var hashids = new HashidsNet.Hashids("Tuwaiq-Internal");
                var id = hashids.Encode(int.Parse(DateTime.Now.ToString("ddMMyyyy")));

                var toBeChecked = new List<CheckList>();

                var options = new RestClientOptions(url)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest($"/check/{id}",Method.Get);
                request.AddHeader("accept", "*/*");
                var collection = await client.GetAsync<List<CheckList>>(request, cancellationToken: stoppingToken);
                if (collection != null) toBeChecked.AddRange(collection);

                // foreach (var item in toBeChecked)
                // {
                //     var response = await checkFactory.GetCheck(item.CheckType).Check(item);
                //
                //     if (response != null )
                //     {
                //         var options2 = new RestClientOptions(url)
                //         {
                //             MaxTimeout = -1,
                //         };
                //         var client2 = new RestClient(options2);
                //         var request2 = new RestRequest($"/save/{id}/{item}", Method.Post);
                //         request2.AddHeader("accept", "*/*");
                //         request2.AddHeader("Content-Type", "application/json");
                //         var body =JsonConvert.SerializeObject(response);
                //
                //         request2.AddStringBody(body, DataFormat.Json);
                //         RestResponse response2 = await client2.PostAsync(request2, cancellationToken: stoppingToken);
                //         if (response2.IsSuccessful)
                //         {
                //             logger.LogInformation($"NationalId: {item} - Result: {JsonConvert.SerializeObject(response)}");
                //         }
                //     }
                // }

                foreach (var item in toBeChecked.GroupBy(s => s.CheckType))
                {
                    var checkLists = item.ToArray().Chunk(20);
                    foreach (var checkList in checkLists)
                    {
                        var response = await checkFactory.GetCheck(item.Key).Check(checkList);
                        if (response != null)
                        {
                            var options2 = new RestClientOptions(url)
                            {
                                MaxTimeout = -1,
                            };
                            var client2 = new RestClient(options2);
                            var request2 = new RestRequest($"/save/{id}/{item.Key}", Method.Post);
                            request2.AddHeader("accept", "*/*");
                            request2.AddHeader("Content-Type", "application/json");
                            var body = JsonConvert.SerializeObject(response);

                            request2.AddStringBody(body, DataFormat.Json);
                            RestResponse response2 = await client2.PostAsync(request2, cancellationToken: stoppingToken);
                            if (response2.IsSuccessful)
                            {
                                logger.LogInformation($"NationalId: {item} - Result: {JsonConvert.SerializeObject(response)}");
                            }
                        }
                    }
                }
                

                logger.LogInformation($"Executed PeriodicHostedService - Count: {_executionCount}");
            }
            catch (Exception ex)
            {
                logger.LogInformation(
                    $"Failed to execute PeriodicHostedService with exception message {ex.Message}. Good luck next round!");
            }
        }
    }

    
}