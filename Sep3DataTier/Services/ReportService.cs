using System.Globalization;
using System.Text;
using Google.Protobuf;
using Model;
using Grpc.Core;
using Sep3DataTier.Database;
using Sep3DataTier.Repository;
using Sep3DataTier.Repository.Intf;

namespace Sep3DataTier.Services;

public class ReportService : Report.ReportBase
{
    private readonly IReportDao reportDao;
    private readonly ILocationDao locationDao;
    private readonly IUserDao userDao;

    public ReportService(IReportDao reportDao, ILocationDao locationDao, IUserDao userDao)
    {
        this.reportDao = reportDao;
        this.locationDao = locationDao;
        this.userDao = userDao;
    }

    public override async Task<ReportList> GetReports(ReportFilter request, ServerCallContext context)
    {
        ICollection<ReportObject> data = new List<ReportObject>();

        IEnumerable<Model.Report> reportsFromDatabase = await reportDao.GetAsync();

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
                User = new UserObject
                {
                    Id = report.User.Id,
                    Username = report.User.UserName
                },
                Status = report.Status,
                Time = new string($"{report.TimeOnly.Hour}:{report.TimeOnly.Minute}:{report.TimeOnly.Second}")
            };
            if (proofIsNull)
                obj.Proof = ByteString.Empty;
            else
                obj.Proof = ByteString.CopyFrom(report.Proof);
            data.Add(obj);
        }

        return await Task.FromResult(new ReportList
        {
            Reports = { data }
        });
    }

    public override async Task<ReportObject> CreateReport(CreateReportObject request, ServerCallContext context)
    {
        ApplicationUser? user = await userDao.GetUserByEmailAsync(request.CreatorEmail);
        Model.Report report = new Model.Report
        {
            DateOnly = DateOnly.ParseExact(request.Date, "yyyy/MM/dd", CultureInfo.InvariantCulture),
            TimeOnly = TimeOnly.Parse(request.Time),
            Proof = request.Proof.ToByteArray(),
            Description = request.Description,
            Status = request.Status,
            Location = new Location()
            {
                Latitude = request.Location.Latitude,
                Longitude = request.Location.Longitude,
                Size = (byte)request.Location.Size
            },
            User = user
        };
        Model.Report result = await reportDao.CreateAsync(report);
        return await Task.FromResult(new ReportObject
        {
            Id = result.Id.ToString(),
            Date = result.DateOnly.ToString("dd/MM/yyyy"),
            Time = result.TimeOnly.ToString("HH:mm"),
            Proof = ByteString.CopyFrom(result.Proof),
            Description = result.Description,
            Status = result.Status,
            Location = new LocationObject
            {
                Latitude = result.Location.Latitude,
                Longitude = result.Location.Latitude,
                Size = result.Location.Size
            },
            User = new UserObject
            {
                Id = result.User.Id,
                Username = result.User.UserName
            }
        });
    }

    public override async Task<ReviewedReport> ReviewReport(ToReviewReport request, ServerCallContext context)
    {
        string confirmation = await reportDao.UpdateReviewAsync(request.ReportId, request.UpdatedStatus);

        return await Task.FromResult(new ReviewedReport
        {
            Confirmation = confirmation
        });
    }
}