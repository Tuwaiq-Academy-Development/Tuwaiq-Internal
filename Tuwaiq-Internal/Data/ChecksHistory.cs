using TuwaiqInternal.Data.Enums;

namespace TuwaiqInternal.Data;

public class ChecksHistory
{
    public int Id { get; set; }
    public string UserId { get; set; } 
    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
    public string? ThirdName { get; set; }
    public string? FourthName { get; set; }
    public string? LastName { get; set; }
    public string? FileUrl { get; set; }
    public string? Status { get; set; }

    public string IdentitiesList { get; set; } 

    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public DateTime? LastUpdate { get; set; }
    public CheckTypes Type { get; set; }
}