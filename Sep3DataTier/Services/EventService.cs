namespace Sep3DataTier.Services;

using System.Globalization;
using Grpc.Core;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Repository;
using Repository.Intf;
using Model;

public class EventService : Sep3DataTier.Event.EventBase
{
    private readonly IEventDao eventDao;
    private readonly ILocationDao locationDao;
    private readonly IReportDao reportDao;
    private readonly IUserDao userDao;

    public EventService(IEventDao eventDao, ILocationDao locationDao, IReportDao reportDao, IUserDao userDao)
    {
        this.eventDao = eventDao;
        this.locationDao = locationDao;
        this.reportDao = reportDao;
        this.userDao = userDao;
    }

    public override async Task<EventObject> CreateEvent(CreateEventObject request, ServerCallContext context)
    {
        ApplicationUser? user = await userDao.GetUserByEmailAsync(request.CreatorEmail);
        Report? report = await reportDao.GetReportByIdAsync(request.ReportId);

        Event cleaningEvent = new Event()
        {
            DateOnly = DateOnly.ParseExact(request.Date, "yyyy/MM/dd", CultureInfo.InvariantCulture),
            TimeOnly = TimeOnly.Parse(request.Time),
            Description = request.Description,
            Status = request.Status,
            Validation = null,
            Organiser = user,
            Report = report
        };

        Event result = await eventDao.CreateEventAsync(cleaningEvent);

        return null;
    }
}