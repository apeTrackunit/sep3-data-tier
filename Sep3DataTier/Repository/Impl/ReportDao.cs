using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Model;
using Sep3DataTier.Database;
using Sep3DataTier.Migrations;

namespace Sep3DataTier.Repository;

public class ReportDao : IReportDao
{
    private readonly DatabaseContext context;

    public ReportDao(DatabaseContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Model.Report>> GetAsync()
    {
        IQueryable<Model.Report> reportsQuery = context.Reports
            .Include(report => report.User)
            .Include(report => report.Location).AsQueryable();
            

        IEnumerable<Model.Report> result = await reportsQuery.ToListAsync();
        return result;
    }

    public async Task<Model.Report> CreateAsync(Model.Report report)
    {
        EntityEntry<Model.Report> result = await context.Reports.AddAsync(report);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<string> UpdateReviewAsync(string reportId, string status)
    {

        
        var foundReport = context.Reports.Include(rep => rep.User).Include(rep => rep.Location).FirstOrDefaultAsync(rep => rep.Id.Equals(Guid.Parse(reportId))).Result;

        if (foundReport == null)
        {
            return await Task.FromResult("Report not found. Status could not be updated!");
        }
        foundReport.Status = status;
        await context.SaveChangesAsync();
        
        return await Task.FromResult("Status updated successfully");
    }

    public async Task<Model.Report> GetByIdAsync(string reportId)
    {
        // for some reason this did not work, but the other one works :^)
        // var foundReport = context.Reports.FirstOrDefault(rep => rep.Id.Equals(Guid.Parse(reportId)));
        
        var foundReport =context.Reports.Where(report => report.Id.Equals(Guid.Parse(reportId))).Include(report => report.User)
            .Include(report => report.Location).FirstOrDefault();
        if (foundReport == null)
            throw new Exception($"Report with {reportId} could not be found!");

        return await Task.FromResult(foundReport);
    }
}