using Entities.Models;

namespace Contracts
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetAllEvents(bool trackChanges = false);
        Event? GetEventById(Guid id, bool trackChanges = false);
        void CreateEvent(Event newEvent);
        void DeleteEvent(Event oldEvent);

    }
}
