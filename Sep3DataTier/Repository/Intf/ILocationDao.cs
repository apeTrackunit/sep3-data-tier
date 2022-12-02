using Model;

namespace Sep3DataTier.Repository.Intf;

public interface ILocationDao
{
    Task<Location> CreateLocationAsync(Location location);
}