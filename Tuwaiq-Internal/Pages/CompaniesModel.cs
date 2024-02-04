using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TuwaiqRecruitment.Pages;

[Authorize(Roles = "admin")]
public class CompaniesModel : PageModel
{
    public void OnGet()
    {

    }

}