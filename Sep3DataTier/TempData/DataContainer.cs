using System.Collections.Generic;
using Model;

namespace TempData;

public class DataContainer
{
    public ICollection<Report> Reports { get; set; }
    public ICollection<Location> Locations { get; set; }
}