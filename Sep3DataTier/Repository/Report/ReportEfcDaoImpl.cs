using Microsoft.EntityFrameworkCore;
using Model;
using Sep3DataTier.Database;

namespace Sep3DataTier.Repository;

public class ReportEfcDaoImpl : IReportEfcDao
{
    private readonly DatabaseContext context;

    public ReportEfcDaoImpl(DatabaseContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Model.Report>> GetAsync()
    {
        IQueryable<Model.Report> reportsQuery = context.Reports.Include(report => report.Location).AsQueryable();

        IEnumerable<Model.Report> result = await reportsQuery.ToListAsync();
        return result;
    }
}