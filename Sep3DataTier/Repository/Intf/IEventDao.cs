namespace Sep3DataTier.Repository.Intf;

using Model;

public interface IEventDao
{
    Task<Event> CreateEventAsync(Model.Event cleaningEvent);
    Task<IEnumerable<Event>> GetEventsAsync(string email, string filter);
    Task<Event> GetEventByIdAsync(string id);
    Task<string> ApproveEventAsync(string id, bool approve);
    Task<string> AttendEventAsync(string id, ApplicationUser user);
    Task<string> SubmitValidation(string id, byte[] validation);
}