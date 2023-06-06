using System.Data.Entity;
using travel_agent.Models;

namespace travel_agent.Infrastructure
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Arrangement> Arrangements { get; set; }
        public DbSet<ArrangementStep> ArrangementSteps { get; set; } 
    }
}
