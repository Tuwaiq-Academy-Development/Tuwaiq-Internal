using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TuwaiqRecruitment.Data;

namespace TuwaiqRecruitment.Pages;

[Authorize(Roles = "admin")]
public class CandidateCreate : PageModel
{
    [BindProperty] public string NationalId { get; set; } = null!;
    
    private readonly ApplicationDbContext _context;

    
    public CandidateCreate(ApplicationDbContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        var companies = await _context.Companies.ToListAsync();
        foreach (var company in companies)
        {
            await _context.Candidates.AddAsync(new Candidate()
            {
                NationalId = NationalId,
                CompanyId = company.Id,
            });
        }
        
        await _context.SaveChangesAsync();
        
        return RedirectToPage("/candidates");
    }
}