using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        IQueryable<string> roleQuery = context.Roles
            .Join(context.UserRoles,
                role => role.Id,
                userRole => userRole.RoleId,
                (role, userRole) => new
                {
                    Role = role,
                    UserRole = userRole
                })
            .Join(context.Users,
                userRole => userRole.UserRole.UserId,
                user => user.Id,
                (userRole, user) => new
                {
                    Role = userRole.Role,
                    UserRole = userRole.UserRole,
                    User = user
                }
            )
            .Where(user => user.User.Email.Equals(email))
            .Select(role => role.Role.Name)
            .AsQueryable();

        IEnumerable<string> roleList = await roleQuery.ToListAsync();
        string role = roleList.ToList()[0];

        if (role.Equals("Admin"))
        {
            reportsQuery = context.Reports
                .Where(report => report.Status.Equals("Under Review"))
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
        else if (role.Equals("User"))
        {
            //Display reports ready for events
            if (approved)
            {
                reportsQuery = context.Reports
                    .Where(report => report.Status.Equals("Approved") &&
                                     !context.Events.Select(e => e.Report.Id).Contains(report.Id))
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
                //Display all report create by me - except the ones that are part of events (by design choice)
                reportsQuery = context.Reports
                    .Where(report => report.User.Email.Equals(email) &&
                                     !context.Events.Select(e => e.Report.Id).Contains(report.Id))
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
        var foundReport = context.Reports
            .FirstOrDefaultAsync(rep => rep.Id.Equals(Guid.Parse(reportId)))
            .Result;

        if (foundReport == null)
            return await Task.FromResult($"Report with {reportId} could not be found! Status could not be updated!");

        foundReport.Status = status;
        await context.SaveChangesAsync();

        return await Task.FromResult("Status updated successfully");
    }

    public async Task<Model.Report> GetReportByIdAsync(string reportId)
    {
        var foundReport = context.Reports
            .Where(report => report.Id.Equals(Guid.Parse(reportId)))
            .Include(report => report.User)
            .Include(report => report.Location)
            .FirstOrDefault();
        if (foundReport == null)
            throw new Exception($"Report with {reportId} could not be found!");

        return await Task.FromResult(foundReport);
    }
}