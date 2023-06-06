using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travel_agent.Models
{
    public class Reservation
    {
        [Key] public int Id { get; set; }
        public User User { get; set; }
        public Arrangement Arrangement { get; set; }

        public Reservation() { }
        public Reservation(User user, Arrangement arrangement)
        {
            User = user;
            Arrangement = arrangement;
        }
    }
}
