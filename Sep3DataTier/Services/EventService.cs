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
            //The validation is always empty upon creation of event
            Validation = ByteString.Empty,
            Organiser = new UserEventObject()
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

    public override async Task<EventList> GetEvents(EventFilter request, ServerCallContext context)
    {
        ICollection<EventObject> events = new List<EventObject>();
        IEnumerable<Event> eventsInDatabase = await eventDao.GetEventsAsync();

        foreach (Event eventObj in eventsInDatabase)
        {
            bool proofIsNull = eventObj.Validation == null;
            EventObject obj = new EventObject
            {
                Id = eventObj.Id.ToString(),
                Date = new string(
                    $"{eventObj.DateOnly.Year:0000}-{eventObj.DateOnly.Month:00}-{eventObj.DateOnly.Day:00}"),
                Time = new string(
                    $"{eventObj.TimeOnly.Hour:00}:{eventObj.TimeOnly.Minute:00}:{eventObj.TimeOnly.Second:00}"),
                Description = eventObj.Description,
                Organiser = new UserEventObject()
                {
                    Id = eventObj.Organiser.Id,
                    Username = eventObj.Organiser.UserName
                },
                Report = new ReportEventObject
                {
                    Description = eventObj.Report.Description,
                    Location = new LocationEventObject
                    {
                        Latitude = eventObj.Report.Location.Latitude,
                        Longitude = eventObj.Report.Location.Longitude,
                        Size = eventObj.Report.Location.Size
                    }
                }
            };
            if (!proofIsNull)
                obj.Validation = ByteString.CopyFrom(eventObj.Validation);
            events.Add(obj);
        }

        return await Task.FromResult(new EventList
        {
            Events = { events }
        });
    }
}