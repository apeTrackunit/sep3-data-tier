// See https://aka.ms/new-console-template for more information

using System.Threading.Channels;
using Model;
using Sep3DataTier.Database;
using Sep3DataTier.Repository;

using Sep3DataTier.Database.DatabaseContext context = new DatabaseContext();

// List<Location> locations = new List<Location>();
// Location location1 = new(4, 5, 3);
// context.Locations.Add(location1);
// context.SaveChangesAsync();

Location locationToAdd = new()
{
    Latitude = 12.3,
    Longitude = 45.6,
    Size = 2
};

Report reportToAdd = new()
{
    Description = "clean",
    Location = locationToAdd,
    Proof = null,
    Status = "Under Review",
    DateOnly = new DateOnly(2022, 11, 16),
    TimeOnly = new TimeOnly(13, 58, 17)
};

//context.Reports.Add(reportToAdd);
//context.SaveChanges();

IReportEfcDao dao = new ReportEfcDaoImpl(new DatabaseContext());

IEnumerable<Report> reports = await dao.GetAsync();
foreach (Report report in reports)
{
    Console.WriteLine(report.Description);
}

