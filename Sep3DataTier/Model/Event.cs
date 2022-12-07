

using System.ComponentModel.DataAnnotations;

namespace Model;

public class Event
{
    [Key]
    public Guid Id { get; set; }
    [DataType(DataType.Date)]
    public DateOnly DateOnly { get; set; }
    [DataType(DataType.Time)]
    public TimeOnly TimeOnly { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public byte[]? Validation { get; set; }
    public ApplicationUser Organiser { get; set; }
    public Report Report { get; set; }

    public Event()
    {
    }
}