using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travel_agent.Models
{
    public class ArrangementStep
    {
        [Key] public int Id { get; set; }
        [ForeignKey("StartPlace")] 
        public int? StartPlaceId { get; set; }
        public Place StartPlace { get; set; }
        [ForeignKey("EndPlace")] 
        public int? EndPlaceId { get; set; }
        public Place EndPlace { get; set; }
        public double TravelDistance { get; set; }
        public TransportType TransportationType { get; set; }
        public enum TransportType
        {
            NONE,
            PLANE,
            TRAIN,
            BUS,
            FOOT
        }
	}


}

