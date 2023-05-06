using System.Data.Entity;
using travel_agent.Models;

namespace travel_agent.Infrastructure
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}
