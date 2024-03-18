using System.ComponentModel.DataAnnotations;
using DAL.Enums;

namespace DAL.Models;

public class CheckList
{
    public Guid Id { get; init; } = Guid.NewGuid();
    [MaxLength(10)] public string NationalId { get; init; } = null!;
    public DateTime CreatedOn { get;private set; } = DateTime.Now;
    public CheckType CheckType { get; init; } = CheckType.Gosi;
}