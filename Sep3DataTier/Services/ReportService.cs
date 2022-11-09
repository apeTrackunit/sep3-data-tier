using System.Text;
using Google.Protobuf;
using Model;
using Grpc.Core;
using TempData;

namespace Sep3DataTier.Services;

public class ReportService : Report.ReportBase
{
    private readonly FileContext context;

    public override Task<ReportList> GetReports(ReportFilter request, ServerCallContext context)
    {

        ICollection<ReportObject> data = new List<ReportObject>();
        ReportObject obj = new ReportObject
        {
            Date = "2020-01-01",
            Description = "govno",
            Location = new LocationObject
            {
                Latitude = 89.01023,
                Longitude = 12.00213,
                Size = 2,
            },
            Proof = ByteString.CopyFrom("Hello world", Encoding.Default),
            Status = "Done",
            Time = "12:12:12"
        };
        
        data.Add(obj);
        
        return Task.FromResult(new ReportList 
        {
            Reports = { data }
        });
    }
}