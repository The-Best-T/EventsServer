using Entities.Models;

namespace Repository.Extensions
{
    public static class RepositoryEventExtensions
    {
        public static IQueryable<Event> FilterEvents(this IQueryable<Event> events, DateTime minDate, DateTime maxDate)
        {
            return events.Where(e => e.Date >= minDate && e.Date <= maxDate);
        }
        public static IQueryable<Event> Search(this IQueryable<Event> events, string searchName)
        {
            if (string.IsNullOrWhiteSpace(searchName))
                return events;
            var lowerCaseName = searchName.Trim().ToLower();

            return events.Where(e => e.Name.ToLower().Contains(lowerCaseName));
        }
    }
}
