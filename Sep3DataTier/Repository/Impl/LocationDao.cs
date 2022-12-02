using Microsoft.EntityFrameworkCore.ChangeTracking;
using Model;
using Sep3DataTier.Database;
using Sep3DataTier.Repository.Intf;

namespace Sep3DataTier.Repository.Impl;

public class LocationDao : ILocationDao
{
    private readonly DatabaseContext context;

    public LocationDao(DatabaseContext context)
    {
        this.context = context;
    }
    
    public async Task<Location> CreateLocationAsync(Location location)
    {
        EntityEntry <Location> createdLocation =  await context.Locations.AddAsync(location);
        await context.SaveChangesAsync();
        return createdLocation.Entity;
    }
}