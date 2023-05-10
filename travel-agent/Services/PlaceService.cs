using System.Collections.Generic;
using System.Linq;
using travel_agent.Models;
using Context = travel_agent.Infrastructure.Context;

namespace travel_agent.Services
{
    public class PlaceService
    {
        private static PlaceService instance = null;
        public static PlaceService Instance
        {
            get
            {
                if (instance == null) instance = new PlaceService();
                return instance;
            }
        }

        public void Create(Place place)
        {
            using (var db = new Context())
            {
                db.Places.Add(place);
                db.SaveChanges();
            }
        }

        public List<Place> GetAll()
        {
            using (var db = new Context()) return db.Places.ToList();
        }

    }
}
