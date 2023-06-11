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

        public int UserId { get; set; }
        public User User { get; set; }

        public int ArrangementId { get; set; }
        public Arrangement Arrangement { get; set; }

        public ReservationStatus Status { get; set; } 
        public DateTime ReservedUntil { get; set; }

        public Reservation() { }
        public Reservation(User user, Arrangement arrangement, ReservationStatus status)
        {
            User = user;
            Arrangement = arrangement;
            Status = status;
        }

        public enum ReservationStatus
        {
            RESERVED,
            PAID,
            CANCELED,
            DELETED
        }
    }
}
