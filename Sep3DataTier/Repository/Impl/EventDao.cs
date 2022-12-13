using MessagePack.Formatters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Sep3DataTier.Repository.Impl;

using Database;
using Intf;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Model;

public class EventDao : IEventDao
{
    private readonly DatabaseContext context;

    public EventDao(DatabaseContext context)
    {
        this.context = context;
    }

    public async Task<Event> CreateEventAsync(Event cleaningEvent)
    {
        EntityEntry<Event> result = await context.Events.AddAsync(cleaningEvent);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<IEnumerable<Event>> GetEventsAsync(string email, string filter)
    {
        IQueryable<Event>? eventQuery = null;

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
            eventQuery = context.Events
                .Where(e => e.Validation != null && !e.Approved)
                .Include(e => e.Organiser)
                .Include(e => e.Report)
                .Include(e => e.Report.Location)
                .Select(e => new Event
                {
                    Id = e.Id,
                    DateOnly = e.DateOnly,
                    TimeOnly = e.TimeOnly,
                    Description = e.Description,
                    Validation = e.Validation,
                    Organiser = e.Organiser,
                    Report = e.Report
                })
                .AsQueryable();
        }
        else if (role.Equals("User"))
        {
            switch (filter)
            {
                case "Organised by me":
                {
                    eventQuery = context.Events
                        .Where(e => e.Organiser.Email.Equals(email))
                        .Include(e => e.Organiser)
                        .Include(e => e.Report)
                        .Include(e => e.Report.Location)
                        .Select(e => new Event
                        {
                            Id = e.Id,
                            DateOnly = e.DateOnly,
                            TimeOnly = e.TimeOnly,
                            Description = e.Description,
                            Validation = e.Validation,
                            Organiser = e.Organiser,
                            Report = e.Report
                        })
                        .AsQueryable();
                    break;
                }
                case "Upcoming":
                {
                    eventQuery = context.Events
                        .Where(e => 
                            e.DateOnly > DateOnly.FromDateTime(DateTime.Now) ||
                            e.DateOnly == DateOnly.FromDateTime(DateTime.Now) &&
                            e.TimeOnly > TimeOnly.FromDateTime(DateTime.Now))
                        .Include(e => e.Organiser)
                        .Include(e => e.Report)
                        .Include(e => e.Report.Location)
                        .Select(e => new Event
                        {
                            Id = e.Id,
                            DateOnly = e.DateOnly,
                            TimeOnly = e.TimeOnly,
                            Description = e.Description,
                            Validation = e.Validation,
                            Organiser = e.Organiser,
                            Report = e.Report
                        })
                        .AsQueryable();
                    break;
                }
                case "Awaiting validation":
                {
                    eventQuery = context.Events
                        .Where(e => 
                            (e.DateOnly < DateOnly.FromDateTime(DateTime.Now) ||
                            e.DateOnly == DateOnly.FromDateTime(DateTime.Now) &&
                            e.TimeOnly < TimeOnly.FromDateTime(DateTime.Now)) &&
                            e.Organiser.Email.Equals(email) &&
                            e.Validation == null)
                        .Include(e => e.Organiser)
                        .Include(e => e.Report)
                        .Include(e => e.Report.Location)
                        .Select(e => new Event
                        {
                            Id = e.Id,
                            DateOnly = e.DateOnly,
                            TimeOnly = e.TimeOnly,
                            Description = e.Description,
                            Validation = e.Validation,
                            Organiser = e.Organiser,
                            Report = e.Report
                        })
                        .AsQueryable();
                    break;
                }
            }
        }

        IEnumerable<Event> result = await eventQuery!.ToListAsync();
        return result;
    }

    public async Task<Event> GetEventByIdAsync(string id)
    {
        Event? foundEvent = context.Events
            .Where(e => e.Id.Equals(Guid.Parse(id)))
            .Include(e => e.Organiser)
            .Include(e => e.Report)
            .Include(e => e.Attendees)
            .Include(e => e.Report.Location)
            .FirstOrDefault();

        if (foundEvent == null)
            throw new Exception($"Event with {id} could not be found!");

        return await Task.FromResult(foundEvent);
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

    public async Task<string> AttendEventAsync(string id, ApplicationUser user)
    {
        var eventToAttend = GetEventByIdAsync(id).Result;

        eventToAttend.Attendees.Add(user);
        await context.SaveChangesAsync();

        return await Task.FromResult("Successfully signed up for an event!");
    }

    public async Task<string> SubmitValidation(string id, byte[] validation)
    {
        Event? foundEvent = context.Events.FirstOrDefaultAsync(e => e.Id.Equals(Guid.Parse(id))).Result;

        if (foundEvent == null)
            return await Task.FromResult($"Event with id: {id} not found!");

        foundEvent.Validation = validation;
        await context.SaveChangesAsync();

        return await Task.FromResult("Validation submitted");
    }
}