using System.Security.Claims;
using System.Text.Json.Nodes;
using IdentityService.SDK;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenIddict.Abstractions;
using OpenIddict.Client.AspNetCore;
using TuwaiqRecruitment.Data;
using TuwaiqRecruitment.ModelView;

namespace TuwaiqRecruitment.Controllers;

// [Authorize]
// public class RecruitmentBaseController : Controller
// {
//     public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//     {
//         if (User.Identity?.IsAuthenticated == true)
//         {
//             var claimsIdentity = User.Identity as ClaimsIdentity;
//
//             if (claimsIdentity != null)
//             {
//                 var claims = claimsIdentity.Claims.ToList();
//                 if (!claims.Any(c => c.Type == "company_id"))
//                 {
//                     var dbContext = HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
//
//                     var userIdClaim = claims.FirstOrDefault(c => c.Type == OpenIddictConstants.Claims.Subject)?.Value;
//                     Guid.TryParse(userIdClaim, out var companyId);
//                     var dbContextCompanies = dbContext.Companies.AsEnumerable()
//                         .FirstOrDefault(c => c.Users != null && c.Users.Contains(companyId))!;
//                     var company = dbContextCompanies;
//                     if (company != null)
//                     {
//                         claims.Add(new Claim("company_id", company.Id.ToString()));
//                         claims.Add(new Claim("company_name", company.Name));
//                         if (!string.IsNullOrEmpty(company.Logo))
//                         {
//                             var logo = "/Storage/" + company?.Logo;
//                             claims.Add(new Claim("company_logo", logo));
//                         }
//                     }
//
//                     var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//                     
//                     //sign in with the new identity
//                     var properties = new AuthenticationProperties { RedirectUri = "/" };
//                     HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);
//                     
//                     // redirect to the same url
//                     return base.OnActionExecutionAsync(context, next);
//                 }
//             }
//         }
//
//         return base.OnActionExecutionAsync(context, next);
//     }
// }

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CandidateController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IResumeUserApi _resumeUserApi;

    public CandidateController(ApplicationDbContext context,IResumeUserApi resumeUserApi)
    {
        _context = context;
        _resumeUserApi = resumeUserApi;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> Get(int page = 1, int pageSize = 10, string query = "")
    {
        var companyIdClaim = User.GetClaim("company_id");
        if (companyIdClaim == null)
            return BadRequest();
        
        Guid.TryParse(companyIdClaim, out var companyId);
        
        if (companyId == Guid.Empty)
            return BadRequest();

        var candidates = await _context.Candidates
            .Where(c => c.CompanyId == companyId)
            .Where(candidate => candidate.NationalId.Contains(query))
            .OrderByDescending(s=>s.InterviewedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        var count = await _context.Candidates
            .Where(c => c.CompanyId == companyId)
            .Where(candidate => candidate.NationalId.Contains(query))
            .CountAsync();

        var res = new PaginationList
        {
            Data = candidates,
            LastPage = (int)Math.Ceiling(count / (double)pageSize)
        };

        return Ok(res);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Create(Candidate candidate)
    {
        var check = JsonConvert.DeserializeObject<Answer>(candidate.Questions);
        candidate.IsSelected = check.مهتم == "نعم";
        await _context.Candidates.AddAsync(candidate);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> Update(Candidate candidate)
    {
        var check = JsonConvert.DeserializeObject<Answer>(candidate.Questions);
        candidate.IsSelected = check.مهتم == "نعم";
        _context.Candidates.Update(candidate);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetCandidate(string nationalId)
    {
        var companyIdClaim = User.GetClaim("company_id");
        if (companyIdClaim == null)
            return BadRequest();
        
        Guid.TryParse(companyIdClaim, out var companyId);
        
        if (companyId == Guid.Empty)
            return BadRequest();
        
        var candidate = await _context.Candidates
            .Where(c => c.NationalId == nationalId && c.CompanyId== companyId)
            .FirstOrDefaultAsync();

        if (candidate == null)
            return NotFound();
        
        var resume = await _resumeUserApi.ResumeView(candidate.NationalId);
        if(resume.IsSuccessStatusCode && resume.Content != null)
        {
            candidate.Name = string.Concat(resume.Content.FirstName, " " ,resume.Content.FatherName, " ", resume.Content.FamilyName);
        }
        
        return Ok(candidate);
    }
}