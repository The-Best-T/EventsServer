using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        public EventRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public void CreateEvent(Event newEvent)
        {
            Create(newEvent);
        }

        public void DeleteEvent(Event oldEvent)
        {
            Delete(oldEvent);
        }

        public IEnumerable<Event> GetAllEvents(bool trackChanges = false)
        {
            return FindAll(trackChanges)
                .OrderBy(e => e.Date)
                .ToList();
        }

        public Event? GetEventById(Guid id, bool trackChanges = false)
        {
            return FindByCondition(e => e.Id.Equals(id), trackChanges).SingleOrDefault();
        }

    }
}
