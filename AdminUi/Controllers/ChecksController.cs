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

        // var currentCandidates = context.CheckList.Where(i => list.Contains(i.NationalId)).ToList();

        if (list.Any())
        {
            var checkRequest = new CheckRequest()
            {
                Username = User.GetName(),
                UserId = User.GetUserId().ToString(),
                Status = "0/" + list.Count,
                FileUrl = "/Storage/Files/" + model.FilePath,
                CheckType = CheckType.Gosi
            };
            context.CheckRequests.Add(checkRequest);
            await context.SaveChangesAsync();

            foreach (var item in list)
            {
                // var current = currentCandidates.FirstOrDefault(i => i.NationalId == item);
                // if (current == null)
                context.CheckList.Add(new CheckList
                {
                    NationalId = item,
                    CheckType = CheckType.Gosi,
                    RequestId = checkRequest.Id
                });
            }

            await context.SaveChangesAsync();
            return Ok();
        }

        return BadRequest("لا يوجد رقم هوية صحيح");
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
        var checkRequest = await context.CheckRequests.FindAsync(id);
        if (checkRequest != null)
        {
            var checkCount = context.CheckLogs.Count(s => s.RequestId == id);
            var all = context.CheckList.Count(s => s.RequestId == id);
            checkRequest.Status = checkCount + "/" + (all + checkCount);

            var timeUtc = DateTime.UtcNow;
            var saTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Riyadh");
            var checkRequestLastUpdate = context.CheckList.Max(s => s.CreatedOn);
            checkRequest.LastUpdate =
                all > 0 ? checkRequestLastUpdate : TimeZoneInfo.ConvertTimeFromUtc(timeUtc, saTimeZone);
            context.CheckRequests.Update(checkRequest);
            await context.SaveChangesAsync();
        }

        return Ok();
    }
}