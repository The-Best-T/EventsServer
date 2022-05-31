using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<Event>> GetAllEventsAsync(bool trackChanges = false)
        {
            return await FindAll(trackChanges)
                .OrderBy(e => e.Date)
                .ToListAsync();
        }

        public async Task<Event?> GetEventByIdAsync(Guid id, bool trackChanges = false)
        {
            return await FindByCondition(e => e.Id.Equals(id), trackChanges)
                .SingleOrDefaultAsync();
        }

    }
}
