using Entities.Models;
using Entities.RequestFeatures;

namespace Contracts
{
    public interface IEventRepository
    {
        Task<PagedList<Event>> GetAllEventsAsync(EventParameters eventParameters,
            bool trackChanges = false);
        Task<Event?> GetEventByIdAsync(Guid id, bool trackChanges = false);
        void CreateEvent(Event newEvent);
        void DeleteEvent(Event oldEvent);

    }
}
