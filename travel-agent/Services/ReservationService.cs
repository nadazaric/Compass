using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using travel_agent.Infrastructure;
using travel_agent.Models;

namespace travel_agent.Services
{
    public class ReservationService
    {
        private static ReservationService instance=null;

        public static ReservationService Instance
        {
            get
            {
                if(instance == null) instance = new ReservationService();
                return instance;
            }
        }

        public void CreateReservation(User user, Arrangement arrangement)
        {
            Reservation reservation = new Reservation(user, arrangement);
            using (var db = new Context())
            {
                db.Reservations.Add(reservation);
                db.SaveChanges();
            }
        }
        public List<Reservation> GetAllForUser(User user)
        {
            using (var db = new Context()) return db.Reservations.Where(r => r.User.Id == user.Id).ToList();
        }
    }
}
