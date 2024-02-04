using System.ComponentModel.DataAnnotations;

namespace TuwaiqRecruitment.ModelView;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    [Display(Name = "Error")] public string Error { get; set; } = null!;

    [Display(Name = "Description")] public string ErrorDescription { get; set; } = null!;
    
    public string RequestedUrl { get; set; } = null!;

    public string RedirectUrl { get; set; }= null!;
    public string ErrorCode { get; set; }= null!;
}