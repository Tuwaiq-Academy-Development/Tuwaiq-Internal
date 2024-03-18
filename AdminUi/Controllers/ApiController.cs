using AdminUi.Helper;
using DAL;
using DAL.Enums;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminUi.Controllers;

[ApiController]
public class ApiController(ApplicationDbContext context) : ControllerBase
{
    
    [HttpPost("[action]/{nationalId}")]
    public async Task<IActionResult> Post(string nationalId)
    {
        var list = new List<string>();
        if (ValidateSAID.check(nationalId) != -1)
        {
            return BadRequest("خطأ في رقم هوية");

        }

        var currentCandidates = context.CheckList.Where(i => list.Contains(i.NationalId)).ToList();
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
        }

        await context.SaveChangesAsync();
        return Ok();
    }
}