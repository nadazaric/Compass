﻿using System.Collections.Generic;
using System.Linq;
using travel_agent.Models;
using Context = travel_agent.Infrastructure.Context;

namespace travel_agent.Services
{
    public class PlaceService
    {
        private static PlaceService instance = null;
        public static PlaceService Instance
        {
            get
            {
                if (instance == null) instance = new PlaceService();
                return instance;
            }
        }

        public void Create(Place place)
        {
            using (var db = new Context())
            {
                db.Places.Add(place);
                db.SaveChanges();
            }
        }

        public void Modify(Place place)
        {
            using (var db = new Context())
            {
                var placeToUpdate = db.Places.FirstOrDefault(p => p.Id == place.Id);
                placeToUpdate.Name = place.Name;
                placeToUpdate.Image = place.Image;
                placeToUpdate.Description = place.Description;
                placeToUpdate.Address = place.Address;
                placeToUpdate.Longitude = place.Longitude;
                placeToUpdate.Latitude = place.Latitude;
                placeToUpdate.Type = place.Type;    
                db.SaveChanges();
            }
        }

        public List<Place> GetAll()
        {
            using (var db = new Context()) return db.Places.ToList();
        }

    }
}
