using System.ComponentModel.DataAnnotations;

namespace Model;

public class Report
{
    [Key]
    public Guid Id { get; set; }
    [DataType(DataType.Date)]
    public DateOnly DateOnly { get; set; }
    [DataType(DataType.Time)]
    public TimeOnly TimeOnly { get; set; }
    public byte[]? Proof { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public Location Location { get; set; }
    public ApplicationUser User { get; set; }
}