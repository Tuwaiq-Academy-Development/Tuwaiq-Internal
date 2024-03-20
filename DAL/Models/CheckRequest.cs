using DAL.Enums;

namespace DAL.Models;

public class CheckRequest
{
    public CheckRequest()
    {
        var timeUtc = DateTime.UtcNow;
        var saTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Riyadh");
        CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, saTimeZone);
    }
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? FileUrl { get; set; } = null!;
    public string? Status { get; set; } = null!;

    public string[] IdentitiesList { get; set; } = null!;
    public DateTime CreatedOn { get; set; } 
    public DateTime? LastUpdate { get; set; }
    public CheckType CheckType { get; set; }
}