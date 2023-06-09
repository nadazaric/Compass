using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using travel_agent.Infrastructure;
using travel_agent.Models;
using Context = travel_agent.Infrastructure.Context;

namespace travel_agent.Services
{
	public class ArrangementService
	{
		private static ArrangementService instance = null;

		public static ArrangementService Instance
		{
			get
			{
				if(instance == null) instance = new ArrangementService();
				return instance;
			}
		}

		public void Create(Arrangement arrangement)
		{
			foreach (var step in arrangement.Steps)
			{
				step.StartPlaceId = step.StartPlace.Id;
				step.StartPlace = null;
				step.EndPlaceId = step.EndPlace.Id;
				step.EndPlace = null;
			}
			using (var db = new Context())
			{

				foreach (var place in arrangement.Places)
				{
					db.Places.Attach(place);
				}

				// Set the state of arrangement and its associated places to Unchanged
				db.Entry(arrangement).State = EntityState.Added;
				foreach (var place in arrangement.Places)
				{
					db.Entry(place).State = EntityState.Unchanged;
				}

				db.SaveChanges();
			}
		}

		public void Modify(Arrangement arrangement)
		{
			using(var db = new Context())
			{
			}
		}

		public List<Arrangement> GetAll()
		{
			using (var db = new Context()) return db.Arrangements.ToList();
		}
	}
}
