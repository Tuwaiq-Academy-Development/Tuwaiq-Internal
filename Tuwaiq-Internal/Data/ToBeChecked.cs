namespace TuwaiqInternal.Data;

public class ToBeChecked
{
    public string NationalId { get; set; }
    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
    public string? ThirdName { get; set; }
    public string? FourthName { get; set; }
    public string? LastName { get; set; }

    public bool IsChecked { get; set; } = false;

    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public DateTime? CheckedOn { get; set; } 
    public string? Response { get; set; }
    public int IsRegistered { get; set; }
}