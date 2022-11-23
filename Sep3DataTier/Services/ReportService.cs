using System.Text;
using Google.Protobuf;
using Model;
using Grpc.Core;
using Sep3DataTier.Database;
using Sep3DataTier.Repository;

namespace Sep3DataTier.Services;

public class ReportService : Report.ReportBase
{
    private readonly IReportEfcDao reportEfcDao;

    public override async Task<ReportList> GetReports(ReportFilter request, ServerCallContext context)
    {
        ICollection<ReportObject> data = new List<ReportObject>();

        IEnumerable<Model.Report> reportsFromDatabase = await reportEfcDao.GetAsync();

        foreach (Model.Report report in reportsFromDatabase)
        {
            ReportObject obj = new ReportObject
            {
                Date = new string($"{report.Date[0]}-{report.Date[1]}-{report.Date[2]}"),
                Description = report.Description,
                Location = new LocationObject
                {
                    Latitude = report.Location.Latitude,
                    Longitude = report.Location.Longitude,
                    Size = report.Location.Size
                },
                Proof = ByteString.CopyFrom(report.Proof),
                Status = report.Status,
                Time = new string($"{report.Time[0]}:{report.Time[1]}:{report.Time[2]}")
            };
            data.Add(obj);
        }

        return await Task.FromResult(new ReportList
        {
            Reports = { data }
        });
    }
}