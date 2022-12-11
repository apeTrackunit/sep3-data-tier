namespace Sep3DataTier.Repository.Intf;

using Model;

public interface IEventDao
{
    Task<Event> CreateEventAsync(Model.Event cleaningEvent);
    Task<List<Event>> GetEventsAsync();
    Task<Event> GetEventAsync(string id);
    Task<string> ApproveEventAsync(string id, bool approve);
}