using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using travel_agent.Infrastructure;
using travel_agent.Models;

namespace travel_agent.Services
{
	public class ArrangementService
	{
		private static ArrangementService instance;

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
			using (var db = new Context())
			{
				db.Arrangements.Add(arrangement);
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
