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

    public override async Task<EventList> GetEvents(EventsFilter request, ServerCallContext context)
    {
        ICollection<EventObject> data = new List<EventObject>();
        IEnumerable<Event> eventsInDatabase = await eventDao.GetEventsAsync(request.Email, request.Filter);

        foreach (Event eventObj in eventsInDatabase)
        {
            EventObject obj = new EventObject
            {
                Id = eventObj.Id.ToString(),
                Date = new string($"{eventObj.DateOnly.Year:0000}-{eventObj.DateOnly.Month:00}-{eventObj.DateOnly.Day:00}"),
                Time = new string($"{eventObj.TimeOnly.Hour:00}:{eventObj.TimeOnly.Minute:00}:{eventObj.TimeOnly.Second:00}"),
                Description = eventObj.Description,
                Organiser = new UserEventObject
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
                },
                Approved = eventObj.Approved
            };
            data.Add(obj);
        }

        return await Task.FromResult(new EventList
        {
            Events = { data }
        });
    }
    
    public override async Task<EventObject> GetEvent(EventFilter request, ServerCallContext context)
    {
        Event eventObj = await eventDao.GetEventAsync(request.Id);
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
            },
            Approved = eventObj.Approved
        };
        if (!proofIsNull) 
            obj.Validation = ByteString.CopyFrom(eventObj.Validation);
        
        return await Task.FromResult(obj);
    }
    
    public override async Task<ApproveEventResult> ApproveEvent(ApproveEventFilter request, ServerCallContext context)
    {
        string confirmation = await eventDao.ApproveEventAsync(request.Id, request.Approve);

        ApproveEventResult response = new ApproveEventResult()
        {
            Confirmation = confirmation
        };
        
        return await Task.FromResult(response);
    }
}