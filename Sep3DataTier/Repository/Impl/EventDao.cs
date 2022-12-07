﻿namespace Sep3DataTier.Repository.Impl;

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
}