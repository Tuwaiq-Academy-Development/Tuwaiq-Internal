using System.ComponentModel.DataAnnotations.Schema;
using HashidsNet;

namespace TuwaiqRecruitment.Data;

public class Candidate
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string NationalId { get; set; } = null!;
    
    [NotMapped]
    public string Name { get; set; } = null!;

    public string Notes { get; set; } = null!;
    public string? Questions { get; set; } = null!;
    public List<string> Tags { get; set; } = null!;

    public bool IsSelected { get; set; } = false;

    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }

    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? InterviewedDate { get; set; }

    [NotMapped]
    public bool IsReviewed  => InterviewedDate.HasValue;

    public string ProfileUrl => new Hashids("Tuwaiq-Profile", 10).EncodeLong(long.Parse(NationalId));
}