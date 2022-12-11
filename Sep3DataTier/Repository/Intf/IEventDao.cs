namespace Sep3DataTier.Repository.Intf;

using Model;

public interface IEventDao
{
    Task<Event> CreateEventAsync(Model.Event cleaningEvent);
    Task<IEnumerable<Event>> GetEventsAsync(string email, string filter);
    Task<Event> GetEventByIdAsync(string id);
    Task<Event> GetEventAsync(string id);
    Task<string> ApproveEventAsync(string id, bool approve);
}