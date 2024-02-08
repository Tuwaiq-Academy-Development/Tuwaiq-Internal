using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TuwaiqRecruitment.Pages;

//[Authorize]
public class IndexModel : PageModel
{
    // private readonly ILogger<IndexModel> _logger;
    //
    // // public class Input
    // // {
    // //     public string FirstNameAr { get; set; } = null!;
    // //     public string SecondNameAr { get; set; } = null!;
    // //     public string FamilyNameAr { get; set; } = null!;
    // //     public string FirstNameEn { get; set; } = null!;
    // //     public string SecondNameEn { get; set; } = null!;
    // //     public string FamilyNameEn { get; set; } = null!;
    // //     public string MobileNumber { get; set; } = null!;
    // //     public string Email { get; set; } = null!;
    // //     public string NationalId { get; set; } = null!;
    // //     public string Gander { get; set; } = null!;
    // //     public string City { get; set; } = null!;
    // //     public string Major { get; set; } = null!;
    // //     public string Degree { get; set; } = null!;
    // //     public string JobStatus { get; set; } = null!;
    // // }
    //
    // [BindProperty(SupportsGet = true)]
    // public Form InputModel { get; set; } = new ();
    //
    // public IndexModel(ILogger<IndexModel> logger)
    // {
    //     _logger = logger;
    // }

    public void OnGet()
    {
    }
    
    // public async Task<IActionResult> OnPostAsync([FromServices] ApplicationDbContext context)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return Page();
    //     }
    //
    //     if (context.Forms.Any(d => d.NationalId == InputModel.NationalId))
    //         return RedirectToPage("./AlreadyRegistered");
    //
    //     try
    //     {
    //         context.Forms.Add(InputModel);
    //         await context.SaveChangesAsync();
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         ModelState.AddModelError("Error", "حدث خطأ أثناء تسجيل البيانات");
    //         return Page();
    //     }
    //
    //     return RedirectToPage("./ThankYou");
    // }
}