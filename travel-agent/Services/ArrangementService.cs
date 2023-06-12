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

		public Arrangement GetOne(Arrangement arrangement)
		{
			using( var db = new Context())
			{
				return db.Arrangements.Include(a=>a.Steps).Include(a => a.Places).First(a => a.Id == arrangement.Id);
			}
		}

		public void DeleteArrangement(Arrangement arrangement)
		{
            using (var db = new Context())
            {
                var arrangementUpdate = db.Arrangements.Include(a => a.Steps).Include(a => a.Places).Single(a => a.Id == arrangement.Id);
				arrangementUpdate.IsDeleted = true;
				db.SaveChanges();
            }
        }

		public void ReactivateArrangement(Arrangement arrangement)
		{
            using (var db = new Context())
            {
                var arrangementUpdate = db.Arrangements.Include(a => a.Steps).Include(a => a.Places).Single(a => a.Id == arrangement.Id);
                arrangementUpdate.IsDeleted = false;
                db.SaveChanges();
            }
        }

		public void Create(Arrangement arrangement)
		{
			arrangement.TotalDistance = 0;
			foreach (var step in arrangement.Steps)
			{
				step.StartPlaceId = step.StartPlace.Id;
				step.StartPlace = null;
				step.EndPlaceId = step.EndPlace.Id;
				step.EndPlace = null;
				arrangement.TotalDistance += step.TravelDistance;
			}
			using (var db = new Context())
			{

				foreach (var place in arrangement.Places)
				{
					db.Places.Attach(place);
				}

				db.Entry(arrangement).State = EntityState.Added;
				foreach (var place in arrangement.Places)
				{
					db.Entry(place).State = EntityState.Unchanged;
				}
				Console.WriteLine("TOTAL " +arrangement.TotalDistance);
				db.SaveChanges();
			}
		}

		public void Modify(Arrangement arrangement)
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
				var arrangementUpdate = db.Arrangements.Include(a=>a.Steps).Include(a=>a.Places).Single(a => a.Id == arrangement.Id);
				arrangementUpdate.Image = arrangement.Image;
				arrangementUpdate.Start = arrangement.Start;
				arrangementUpdate.End = arrangement.End;
				arrangementUpdate.Name = arrangement.Name;
				arrangementUpdate.Price = arrangement.Price;
				arrangementUpdate.TotalDistance = arrangement.TotalDistance;
				arrangementUpdate.Description = arrangement.Description;

				arrangementUpdate.Steps.Clear();

				foreach (var step in arrangement.Steps)
				{
					if(step.Id == 0)
					{
						db.ArrangementSteps.Add(step);
						Console.WriteLine(step.StartPlaceId + " " + step.EndPlaceId);
						db.Entry(step).State = EntityState.Added;
					}
					else
					{
						var existingStep = arrangementUpdate.Steps.SingleOrDefault(s => s.Id == step.Id);
						if (existingStep != null)
						{
							db.ArrangementSteps.Attach(existingStep);
							db.Entry(existingStep).State = EntityState.Modified;
							db.Entry(existingStep).CurrentValues.SetValues(step);
						}
					}
					arrangementUpdate.Steps.Add(step);
				}

				var places = new List<Place>(arrangement.Places);
				arrangementUpdate.Places.Clear();

				foreach (var place in places)
				{

					Place existingPlace = db.Places.Include(x =>x.Arrangements).Single(x => x.Id == place.Id);
					if (existingPlace != null)
					{
						if (!existingPlace.Arrangements.Any(a => a.Id == arrangement.Id))
						{
							existingPlace.Arrangements.Add(arrangementUpdate);
							db.Entry(existingPlace).State = EntityState.Modified;
						}
						arrangementUpdate.Places.Add(existingPlace);
					}

				}
				
				db.Entry(arrangementUpdate).State = EntityState.Modified;


				db.SaveChanges();
			}
		}

		public ArrangementStep GetByPlacesAndArrangement(int startId, int endId, int ArrangementId)
		{
			using(var db = new Context())
			{
				var Arrangement = db.Arrangements.Include(a=>a.Steps).Include(a=>a.Places).First(a => a.Id == ArrangementId);
				foreach(var step in Arrangement.Steps)
				{
					if (step.StartPlaceId == startId && step.EndPlaceId == endId)
					{
						return step;
					}
					
				}
				return null;
			}
		}

		public List<Arrangement> GetFuture()
		{
			using(var db = new Context()) { return db.Arrangements.Include(a => a.Places).Include(a => a.Steps).Where(a => DateTime.Compare(a.Start, DateTime.Now)>0).ToList(); }
		}
		public List<Arrangement> GetAll()
		{
			using (var db = new Context()) return db.Arrangements.Include(a => a.Places).Include(a => a.Steps).ToList();
		}

	}
}
