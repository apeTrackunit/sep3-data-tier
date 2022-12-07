namespace Sep3DataTier.Repository.Intf;

using Model;

public interface IEventDao
{
    Task<Event> CreateEventAsync(Model.Event cleaningEvent);
    Task<List<Event>> GetEventsAsync();
}