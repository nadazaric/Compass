using System.Linq;
using travel_agent.Models;
using Context = travel_agent.Infrastructure.Context;

namespace travel_agent.Services
{
    internal class UserService
    {
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

        public bool TryLogin(string email, string password)
        {
            using(var db = new Context())
            {
                User user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
                if (user == null) return false;
                return true;
            }
        }
    }
}
