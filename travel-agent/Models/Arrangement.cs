using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travel_agent.Models
{

	public class Arrangement
	{
		[Key] public int Id { get; set; }
		public List<Place> Places { get; set; }
		public List<ArrangementStep> Steps { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public string Name { get; set; }
		public double TotalDistance { get; set; }

		public Arrangement()
		{
			Places = new List<Place>();
			Steps = new List<ArrangementStep>();
		}


	}
}
