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

    public ReportService(IReportEfcDao reportEfcDao)
    {
        this.reportEfcDao = reportEfcDao;
    }

    public override async Task<ReportList> GetReports(ReportFilter request, ServerCallContext context)
    {
        ICollection<ReportObject> data = new List<ReportObject>();

        IEnumerable<Model.Report> reportsFromDatabase = await reportEfcDao.GetAsync();

        foreach (Model.Report report in reportsFromDatabase)
        {
            bool proofIsNull = report.Proof == null;
            ReportObject obj = new ReportObject
            {
                Date = new string($"{report.DateOnly.Year}-{report.DateOnly.Month}-{report.DateOnly.Day}"),
                Description = report.Description,
                Location = new LocationObject
                {
                    Latitude = report.Location.Latitude,
                    Longitude = report.Location.Longitude,
                    Size = report.Location.Size
                },
                Proof = ByteString.CopyFrom(report.Proof) ,
                Status = report.Status,
                Time = new string($"{report.TimeOnly.Hour}:{report.TimeOnly.Minute}:{report.TimeOnly.Second}")
            };
            data.Add(obj);
        }

        return await Task.FromResult(new ReportList
        {
            Reports = { data }
        });
    }
}