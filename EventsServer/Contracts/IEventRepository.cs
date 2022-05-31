using Entities.Models;

namespace Contracts
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEventsAsync(bool trackChanges = false);
        Task<Event?> GetEventByIdAsync(Guid id, bool trackChanges = false);
        void CreateEvent(Event newEvent);
        void DeleteEvent(Event oldEvent);

    }
}
