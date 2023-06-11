using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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
            Reservation reservation = new Reservation(user, arrangement, Reservation.ReservationStatus.RESERVED);
            reservation.User = null;
            reservation.UserId = user.Id;
            reservation.Arrangement = null;
            reservation.ArrangementId = arrangement.Id;
            using (var db = new Context())
            {
                db.Reservations.Add(reservation);
                db.SaveChanges();
            }
        }
        public List<Reservation> GetAllForUser(User user, bool future)
        {
			using (Context db = new Context())
			{
				return db.Reservations.Include(r => r.Arrangement).Include(r => r.User)
                    .Where( r => r.User.Id == user.Id && future ? (DateTime.Compare(r.Arrangement.End, DateTime.Now)>0) : (DateTime.Compare(r.Arrangement.End, DateTime.Now) < 0))
                    .ToList();
			}
		}

        public void CancelReservationForUser(Reservation reservation) 
        {
            using (var db = new Context())
            {
                Reservation res = db.Reservations.Find(reservation.Id);
                if(res != null)
                {
                    res.Status = Reservation.ReservationStatus.CANCELED;
                    db.SaveChanges();
                } 
                else
                {
                    Console.WriteLine("Greska prilikom otkazivanja");
                }
            }
        }

        public void PaidReservation(User user, Reservation reservation)
        {
            using (var db = new Context())
            {
                Reservation res = db.Reservations.Find(reservation.Id);
                if (res != null)
                {
                    res.Status = Reservation.ReservationStatus.PAID;
                    db.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Greska prilikom otkazivanja");
                }
            }
        }
    }
}
