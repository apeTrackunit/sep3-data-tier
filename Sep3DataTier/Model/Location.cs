using System.ComponentModel.DataAnnotations;

namespace Model;

public class Location
{
    [Key]
    public int Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public byte Size { get; set; }
}