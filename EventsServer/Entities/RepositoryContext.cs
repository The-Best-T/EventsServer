using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class RepositoryContext : DbContext
    {
        private DbSet<Event> Events { get; set; } = null!;
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

    }
}