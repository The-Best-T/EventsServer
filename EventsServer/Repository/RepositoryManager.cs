using Contracts;
using Entities;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private IEventRepository _eventRepository;

        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
        }
        public IEventRepository Event
        {
            get
            {
                if (_eventRepository == null)
                    _eventRepository = new EventRepository(_context);
                return _eventRepository;
            }
        }

        public Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
