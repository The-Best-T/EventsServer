using Entities.Models;

namespace Contracts
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetAllEvents(bool trackChanges);
        Event? GetEventById(Guid id, bool trackChanges);
        void CreateEvent(Event newEvent);
        void DeleteEvent(Event oldEvent);

    }
}
