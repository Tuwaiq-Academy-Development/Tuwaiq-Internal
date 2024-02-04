namespace TuwaiqRecruitment.Data;

public class Company
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string Logo { get; set; } = null!;

    public List<Candidate> Candidates = new();

    public List<Guid>? Users { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}