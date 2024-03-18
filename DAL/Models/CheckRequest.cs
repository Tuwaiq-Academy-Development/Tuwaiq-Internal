using DAL.Enums;

namespace DAL.Models;

public class CheckRequest
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? FileUrl { get; set; } = null!;
    public string? Status { get; set; } = null!;

    public string[] IdentitiesList { get; set; } = null!;
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public DateTime? LastUpdate { get; set; }
    public CheckType CheckType { get; set; }
}