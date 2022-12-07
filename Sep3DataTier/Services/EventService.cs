using Google.Protobuf;

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

        EventObject reply = new EventObject()
        {
            Id = result.Id.ToString(),
            Date = report.DateOnly.ToString("yyyy-MM-dd"),
            Time = new string($"{report.TimeOnly.Hour:00}:{report.TimeOnly.Minute:00}:{report.TimeOnly.Second:00}"),
            Description = result.Description,
            Status = result.Status,
            //The validation is always empty upon creation of event
            Validation = ByteString.Empty,
            Organiser = new EventUserObject
            {
                Id = result.Organiser.Id,
                Username = result.Organiser.UserName
            },
            Report = new ReportEventObject
            {
                Description = result.Report.Description,
                Proof = ByteString.CopyFrom(result.Report.Proof),
                Location = new LocationEventObject
                {
                    Latitude = result.Report.Location.Latitude,
                    Longitude = result.Report.Location.Longitude,
                    Size = result.Report.Location.Size,
                }
            }
        };
        
        return reply;
    }
}