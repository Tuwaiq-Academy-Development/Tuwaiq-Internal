using System.Security.Claims;
using System.Text.Json.Nodes;
using IdentityService.SDK;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TuwaiqInternal.Helper;
using TuwaiqInternal.Data;
using TuwaiqInternal.Data.Enums;
using TuwaiqRecruitment.ModelView;

namespace TuwaiqRecruitment.Controllers;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CandidateController(ApplicationDbContext context) : Controller
{
    [HttpPost("[action]")]
    public async Task<IActionResult> CheckIdentities(List<string> candidates)
    {
        var list = new List<string>();
        foreach (var item in candidates)
        {
            if (ValidateSAID.check(item) != -1)
            {
                list.Add(item);
            }
        }
        
        var currentCandidates = context.ToBeCheckeds.Where(i => list.Contains(i.NationalId)).ToList();
        var validCandidates = new List<string>();
        foreach (var item in list)
        {
            var current = currentCandidates.FirstOrDefault(i => i.NationalId == item);
            if (ValidateSAID.check(item) != -1)
            {
                if (current != null)
                {
                    current.IsChecked = false;
                }
                else
                {
                    context.ToBeCheckeds.Add(new ToBeChecked()
                    {
                        NationalId = item,
                        IsChecked = false,
                        CreatedOn = DateTime.Now,
                        IsRegistered = 0
                    });
                }

                validCandidates.Add(item);
            }
        }

        context.ChecksHistories.Add(new ChecksHistory()
        {
            CreatedOn = DateTime.Now,
            LastUpdate = DateTime.Now,
            IdentitiesList = Newtonsoft.Json.JsonConvert.SerializeObject(validCandidates),
            FirstName = "test",
            SecondName = "test",
            ThirdName = "test",
            LastName = "test",
            UserId = "1",
            Status = "0/" + list.Count,
            // FileUrl = "/templates/",
            Type = CheckTypes.Hadaf
        });
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetHistory(int? page = 1, int? size = 10)
    {
        var result = context.ChecksHistories.Select(x => new
        {
            x.UserId,
            x.Id, x.FirstName, x.FileUrl, x.Status, x.CreatedOn, x.LastUpdate, x.Type
        }).Skip(((page ?? 1) - 1) * (size ?? 10)).Take(size ?? 10).ToList();
        var count = context.ChecksHistories.Count();
        return Ok(new
        {
            Data = result.ToList(),
            Last_page = (int)Math.Ceiling(count / (double)size)
        });
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateStatus(int id)
    {
        var toBeupdates = context.ChecksHistories.FirstOrDefault(i => i.Id ==  id);
        var serializedList =JsonConvert.DeserializeObject<List<string>>(toBeupdates.IdentitiesList) ;
        var checkCount =    context.ToBeCheckeds.Count(i => serializedList.Contains(i.NationalId) && i.IsChecked == true);
         toBeupdates.Status = checkCount + "/" + serializedList.Count;
        await context.SaveChangesAsync();
        return Ok();
    }
}