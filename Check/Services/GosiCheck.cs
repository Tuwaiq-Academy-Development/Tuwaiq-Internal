using DAL.Enums;
using DAL.Models;
using Newtonsoft.Json;

namespace Check.Services;

public class GosiCheck : ICheck
{
    private readonly ILogger<GosiCheck> _logger;

    public GosiCheck(/*IGOSIEmploymentStatusService employmentStatusService,*/ ILogger<GosiCheck> logger)
    {
        _logger = logger;
    }

    public async Task<CheckLog?> Check(CheckList item)
    {
       var employmentStatusService = new GOSIEmploymentStatusServiceClient();

        var result = await employmentStatusService.GetEmploymentStatusAsync(item.NationalId);
        _logger.LogInformation($"NationalId: {item} - Result: {JsonConvert.SerializeObject(result)}");
        var response = new CheckLog
        {
            Id = item.Id,
            Response = JsonConvert.SerializeObject(result),
            CheckedOn = DateTime.Now,
            NationalId = item.NationalId!,
            CheckType = CheckType.Gosi,
        };
        if (result.Item is EmploymentStatusStructure structure)
        {
            response.Status = GetStatusAsync(structure);
        }

        return response;
    }

    public async Task<CheckLog[]?> Check(CheckList[] item)
    {
        var employmentStatusService = new GOSIEmploymentStatusServiceClient();
        var result = await employmentStatusService.GetEmploymentStatusMultipleAsync(item.Select(x => x.NationalId).ToArray());
        var response = new List<CheckLog>();

        var list = result.Item as MultipleEmploymentStatusStructure;

        foreach (var res in list?.EmploymentStatusStructure.ToList()!)
        {
            var nationalId = res.Contributor.ContributorID.NationalID;

            var log = new CheckLog
            {
                Id = item.First(x => x.NationalId == nationalId).Id,
                Response = JsonConvert.SerializeObject(res),
                CheckedOn = DateTime.Now,
                NationalId = nationalId,
                CheckType = CheckType.Gosi,
                Status = GetStatusAsync(res)
            };
            response.Add(log);
        }
        
        return response.ToArray();
    }

    public string GetStatusAsync(object result) =>
        ((EmploymentStatusStructure)result).ContributorStatus switch
        {
            0 => "غير مشترك",
            1 => "غير محدث | لا يعمل",
            2 => "يعمل",
            _ => "Unknown"
        };
}