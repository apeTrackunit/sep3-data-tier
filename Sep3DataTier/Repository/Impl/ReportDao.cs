using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Model;
using Sep3DataTier.Database;

namespace Sep3DataTier.Repository;

public class ReportDao : IReportDao
{
    private readonly DatabaseContext context;

    public ReportDao(DatabaseContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Model.Report>> GetAsync(string email, bool approved)
    {
        IQueryable<Model.Report>? reportsQuery = null;
        if (approved)
        {
            reportsQuery = context.Reports
                .Where(report => report.Status.Equals("Approved"))
                .Include(report => report.User)
                .Include(report => report.Location)
                .Select(r => new Model.Report
                {
                    Id = r.Id,
                    DateOnly = r.DateOnly,
                    TimeOnly = r.TimeOnly,
                    Description = r.Description,
                    User = r.User,
                    Status = r.Status,
                    Location = r.Location
                })
                .AsQueryable();
        }
        else
        {
            reportsQuery = context.Reports
                .Where(report => report.User.Email.Equals(email))
                .Include(report => report.User)
                .Include(report => report.Location)
                .Select(r => new Model.Report
                {
                    Id = r.Id,
                    DateOnly = r.DateOnly,
                    TimeOnly = r.TimeOnly,
                    Description = r.Description,
                    User = r.User,
                    Status = r.Status,
                    Location = r.Location
                })
                .AsQueryable();
        }
        IEnumerable<Model.Report> result = await reportsQuery!.ToListAsync();

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
        var foundReport = context.Reports.FirstOrDefaultAsync(rep => rep.Id.Equals(Guid.Parse(reportId))).Result;

        if (foundReport == null)
        {
            return await Task.FromResult("Report not found. Status could not be updated!");
        }

        foundReport.Status = status;
        await context.SaveChangesAsync();

        return await Task.FromResult("Status updated successfully");
    }

    public async Task<Model.Report> GetReportByIdAsync(string reportId)
    {
        
        var foundReport =context.Reports.Where(report => report.Id.Equals(Guid.Parse(reportId))).Include(report => report.User)
            .Include(report => report.Location).FirstOrDefault();
        if (foundReport == null)
            throw new Exception($"Report with {reportId} could not be found!");

        return await Task.FromResult(foundReport);
    }
}