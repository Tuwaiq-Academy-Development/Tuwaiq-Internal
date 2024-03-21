using System.ComponentModel.DataAnnotations;
using DAL.Enums;

namespace DAL.Models;

public class CheckList
{
    public CheckList()
    {
        // get datetime in Riyadh timezone
        var timeUtc = DateTime.UtcNow;
        var saTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Riyadh");
        CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, saTimeZone);
    }
    public Guid Id { get; init; } = Guid.NewGuid();
    [MaxLength(10)] public string NationalId { get; init; } = null!;
    public DateTime CreatedOn { get;private set; }
    public CheckType CheckType { get; init; } = CheckType.Gosi;

    public int? RequestId { get; set; }
}