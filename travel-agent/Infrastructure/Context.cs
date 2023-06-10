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
        public DbSet<Reservation> Reservations { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
            modelBuilder.Entity<Arrangement>()
                .HasMany(a => a.Places)
                .WithMany(a => a.Arrangements)
                .Map(m =>
                {
                    m.ToTable("ArrangementPlaces");
                    m.MapLeftKey("ArrangementId");
                    m.MapRightKey("PlaceId");
                });

			base.OnModelCreating(modelBuilder);
		}
	}

}
