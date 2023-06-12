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

		public List<Reservation> GetAllForUser(User user, bool future)
		{
			using (Context db = new Context())
			{
				return db.Reservations.Include(r => r.Arrangement).Include(r => r.User)
					.Where(r => r.User.Id == user.Id && future ? (DateTime.Compare(r.Arrangement.End, DateTime.Now) > 0) : (DateTime.Compare(r.Arrangement.End, DateTime.Now) < 0))
					.ToList();
			}
		}

		public Reservation GetUserReservation(User user, Arrangement arrangement)
		{
			using (Context db = new Context())
			{
				return db.Reservations.Include(r => r.Arrangement).Include(r => r.User).SingleOrDefault(r => r.UserId == user.Id && r.ArrangementId == arrangement.Id);
			}
		}

		public Reservation CreateReservation(User user, Arrangement arrangement)
        {
            Reservation reservation = new Reservation(user, arrangement, Reservation.ReservationStatus.RESERVED);
            reservation.User = null;
            reservation.UserId = user.Id;
            reservation.Arrangement = null;
            reservation.ArrangementId = arrangement.Id;
            reservation.ReservedUntil = DateTime.Now.AddDays(3);
            using (var db = new Context())
            {
                db.Reservations.Add(reservation);
                db.SaveChanges();
                return reservation;
            }
        }

        public void RecreateReservation(Reservation reservation)
        {
            using(var db = new Context())
            {
                Reservation res = db.Reservations.Find(reservation.Id);
                if(res != null)
                {
                    res.Status = Reservation.ReservationStatus.RESERVED;
                    reservation.ReservedUntil = DateTime.Now.AddDays(3);
                    db.SaveChanges();
                }
               
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

        public void PayReservation(Reservation reservation)
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

        public void UpdateReservationsStatus()
        {
            using(var db = new Context())
            {
                foreach(var reservation in db.Reservations.Where(r => r.Status == Reservation.ReservationStatus.RESERVED))
                {
                    if(DateTime.Compare(DateTime.Now, reservation.ReservedUntil) >= 0)
                    {
                        reservation.Status = Reservation.ReservationStatus.DELETED;
                    }
                }

                db.SaveChanges();
            }
        }

       
    }
}
