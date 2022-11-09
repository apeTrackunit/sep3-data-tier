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
        
        return Task.FromResult(new ReportList 
        {
            Reports = { data }
        });
    }
}