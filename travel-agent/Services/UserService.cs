using System;
using System.Linq;
using travel_agent.Models;
using Context = travel_agent.Infrastructure.Context;

namespace travel_agent.Services
{
    internal class UserService
    {
        private static UserService instance = null;
        public static UserService Instance
        {
            get
            {
                if(instance == null) instance = new UserService();
                return instance;
            }
        }

        public void InitContext()
        {
            using (var db = new Context()) db.Users.ToList();
        }

        public void CreateDefaultAgnt()
        {
            using (var db = new Context())
            {
                User temp = new User { Name = "Miki", LastName = "Kocka", Email = "m@mail.com", Password = "123123", Role = Role.PASSENGER };
                db.Users.Add(temp);
                db.Users.Add(new User { Name = "Vanja", LastName = "Kocka", Email = "vanja@mail.com", Password = "vanja", Role = Role.AGENT });
               
                Arrangement arr = new Arrangement();
                arr.Name = "t";
                arr.Places.Add(new Place());
                arr.Steps.Add(new ArrangementStep());
                arr.Start = DateTime.Now;
                arr.End = DateTime.Now;
                arr.TotalDistance = 0;
                arr.Price = 0;
                db.Arrangements.Add(arr);
                db.Reservations.Add(new Reservation ( temp, arr, Reservation.ReservationStatus.RESERVED));
                db.SaveChanges();
            }
        }

        public void Create(User user)
        {
            using(var db = new Context())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        public bool IsEmailAlreadyUsed(string email)
        {
            using(var db = new Context())
            {
                int count = db.Users.Where(u => u.Email == email).Count();
                return count > 0;
            }
        }

        public User TryLogin(string email, string password)
        {
            using(var db = new Context()) return db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);    
        }

        public void InitialSetup()
		{
            using(var db = new Context())
			{
                var users = db.Users.ToList();
                if (users.Count == 0) CreateDefaultAgnt();
			}
		}
    }
}
