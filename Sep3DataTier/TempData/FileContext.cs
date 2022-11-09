using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using Model;

namespace TempData;

public class FileContext
{
    private const string filePath = "data.json";
    private DataContainer? dataContainer;

    public ICollection<Report> Reports
    {
        get
        {
            LoadData();
            return dataContainer!.Reports;
        }
    }

    public ICollection<Location> Locations
    {
        get
        {
            LoadData();
            return dataContainer!.Locations;
        }
    }

    private void LoadData()
    {
        if (dataContainer != null) return;

        if (!File.Exists(filePath))
        {
            dataContainer = new()
            {
                Reports = new List<Report>(),
                Locations = new List<Location>()
            };
            return;
        }

        string content = File.ReadAllText(filePath);
        dataContainer = JsonSerializer.Deserialize<DataContainer>(content);
    }

    public void SaveChanges()
    {
        string serialized = JsonSerializer.Serialize(dataContainer);
        File.WriteAllText(filePath, serialized);
        dataContainer = null;
    }
}