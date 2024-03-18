namespace AdminUi.Dto;

public class CheckIdentitiesDto
{ 
    public List<string> NationalIds { get; set; } = new List<string>();

    public string FilePath { get; set; } = null!;
}