namespace IAM.Application.Consumers;

public class CheckGosi
{
    public CheckGosi(string nationalId)
    {
        NationalId = nationalId;
    }

    public string NationalId { get; set; }
}

public class CheckGosiResponse
{
    public CheckGosiResponse(string nationalId, DateTime? checkedOn, string checkType, string status)
    {
        NationalId = nationalId;
        CheckedOn = checkedOn;
        CheckType = checkType;
        Status = status;
    }

    public string NationalId { get; set; }
    public DateTime? CheckedOn { get; set; }
    public string CheckType { get; set; }
    public string Status { get; set; } = null!;
}