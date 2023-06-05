using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travel_agent.Models
{
	public class ArrangementStep
	{
		[Key] public int Id { get; set; }
        public Place StartPlace { get; set; }
        public Place EndPlace { get; set; }
        public double TravelDistance { get; set; }
        public TransportType Type { get; set; }
        public enum TransportType
        {
            PLANE,
            TRAIN,
            BUS,
            FOOT
        }
    }
}

