using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

    public async Task<IEnumerable<Model.Report>> GetAsync()
    {
        IQueryable<Model.Report> reportsQuery = context.Reports.Include(report => report.Location).AsQueryable();

        IEnumerable<Model.Report> result = await reportsQuery.ToListAsync();
        return result;
    }

    public async Task<Model.Report> CreateAsync(Model.Report report)
    {
        EntityEntry<Model.Report> result = await context.Reports.AddAsync(report);
        await context.SaveChangesAsync();
        return result.Entity;
    }
}