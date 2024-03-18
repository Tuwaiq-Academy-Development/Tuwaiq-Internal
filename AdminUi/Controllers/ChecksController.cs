using AdminUi.Dto;
using AdminUi.Helper;
using DAL;
using DAL.Enums;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminUi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ChecksController(ApplicationDbContext context) : Controller
{
    [HttpPost("[action]")]
    public async Task<IActionResult> CheckIdentities(CheckIdentitiesDto model)
    {
        var list = new List<string>();
        foreach (var item in model.NationalIds.Where(s => s.Length == 10).Distinct())
        {
            if (ValidateSAID.check(item) != -1)
            {
                list.Add(item);
            }
        }

        if (list.Count == 0)
        {
            return BadRequest("لا يوجد رقم هوية صحيح");
        }

        var currentCandidates = context.CheckList.Where(i => list.Contains(i.NationalId)).ToList();
        var validCandidates = new List<string>();
        foreach (var item in list)
        {
            var current = currentCandidates.FirstOrDefault(i => i.NationalId == item);
            if (current == null)
            {
                context.CheckList.Add(new CheckList
                {
                    NationalId = item,
                    CheckType = CheckType.Gosi
                });
            }

            validCandidates.Add(item);
        }

        context.CheckRequests.Add(new CheckRequest()
        {
            CreatedOn = DateTime.Now,
            LastUpdate = DateTime.Now,
            IdentitiesList = validCandidates.ToArray(),
            Username = User.GetName(),
            UserId = User.GetUserId().ToString(),
            Status = "0/" + list.Count,
            FileUrl = "/Storage/Files/" + model.FilePath,
            CheckType = CheckType.Gosi
        });
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetHistory(int? page = 1, int? size = 10)
    {
        var result = context.CheckRequests.Select(x => new
        {
            x.UserId, x.Id, x.Username, x.FileUrl, x.Status, x.CreatedOn, x.LastUpdate,
            Type = x.CheckType
        }).OrderByDescending(s => s.CreatedOn).Skip(((page ?? 1) - 1) * (size ?? 10)).Take(size ?? 10).ToList();
        var count = await context.CheckRequests.CountAsync();
        return Ok(new
        {
            Data = result.ToList(),
            Last_page = (int)Math.Ceiling(count / (double)size!)
        });
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateStatus(int id)
    {
        var toBeupdates = context.CheckRequests.FirstOrDefault(i => i.Id == id);
        if (toBeupdates != null)
        {
            var serializedList = toBeupdates.IdentitiesList;
            var checkCount =
                context.CheckList.Count(i => serializedList.Contains(i.NationalId) && i.CheckType == toBeupdates.CheckType);
            toBeupdates.Status = checkCount + "/" + serializedList.Length;
            toBeupdates.LastUpdate = DateTime.Now;
            await context.SaveChangesAsync();
        }

        return Ok();
    }
}