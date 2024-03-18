using DAL.Enums;

namespace DAL.Models;

public class CheckLog
{
    public Guid Id { get; set; } 
    public string NationalId { get; set; } = null!;
    public DateTime? CheckedOn { get; set; }
    public string? Response { get; set; } = null!;
    public CheckType CheckType { get; set; }
    public string Status { get; set; } = null!;
}