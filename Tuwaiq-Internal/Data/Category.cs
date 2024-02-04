namespace TuwaiqRecruitment.Data;

public class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    
}