using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;

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

        public async Task<PagedList<Event>> GetAllEventsAsync(EventParameters eventParameters,
            bool trackChanges = false)
        {
            var events = await FindAll(trackChanges)
                .FilterEvents(eventParameters.MinDate, eventParameters.MaxDate)
                .Search(eventParameters.SearchName)
                .OrderBy(e => e.Date)
                .Skip((eventParameters.PageNumber - 1) * eventParameters.PageSize)
                .Take(eventParameters.PageSize)
                .ToListAsync();

            var count = await FindAll(false).CountAsync();

            return PagedList<Event>
                .ToPagedList(events, eventParameters.PageNumber, eventParameters.PageSize, count);
        }

        public async Task<Event?> GetEventByIdAsync(Guid id, bool trackChanges = false)
        {
            return await FindByCondition(e => e.Id.Equals(id), trackChanges)
                .SingleOrDefaultAsync();
        }
    }
}
