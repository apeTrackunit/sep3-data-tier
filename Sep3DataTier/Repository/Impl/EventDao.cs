using Microsoft.EntityFrameworkCore;

namespace Sep3DataTier.Repository.Impl;

using Database;
using Intf;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Model;

public class EventDao:IEventDao
{
    private readonly DatabaseContext context;

    public EventDao(DatabaseContext context)
    {
        this.context = context;
    }
    
    public async Task<Event> CreateEventAsync(Event cleaningEvent)
    {
        EntityEntry<Model.Event> result = await context.Events.AddAsync(cleaningEvent);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<List<Event>> GetEventsAsync()
    {
        List<Event> events = await context.Events
            .Include(ev => ev.Report.Location).ToListAsync();
        return events;
    }

    public async Task<Event> GetEventAsync(string id)
    {
        var eventObj = await context.Events
            .Where(ev => ev.Id.Equals(Guid.Parse(id)))
            .Include(ev => ev.Report.Location)
            .Include(ev => ev.Organiser).FirstOrDefaultAsync();
        if (eventObj == null)
            throw new Exception($"Event with {id} could not be found!");
        
        return eventObj;
    }

    public async Task<string> ApproveEventAsync(string id, bool approve)
    {
        var eventObject = context.Events.FirstOrDefaultAsync(ev => ev.Id.Equals(Guid.Parse(id))).Result;

        if (eventObject == null)
        {
            throw new Exception($"Event with {id} could not be found!");
        }
        
        if (approve)
        {
            eventObject.Approved = approve;
        }
        else
        {
            context.Events.Remove(eventObject);
        }
        
        await context.SaveChangesAsync();

        return await Task.FromResult("Event approval updated!");
    }
}