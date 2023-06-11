using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;

namespace travel_agent.Controls
{
    public class Geocoder
    {
        private static string KEY = null;
        private static Geocoder instance = null;
        public static Geocoder Instance
        {
            get
            {
                if (instance == null) instance = new Geocoder();
                return instance;
            }
        }

        private string GetKey()
        {
            try 
            {
                KEY = File.ReadAllText("../../mapkey.txt");
                return KEY;
            }
            catch (FileNotFoundException) { return string.Empty; }
        }

        public async Task<double> CalculateDistanceAsync(string origin, string destination)
        {
			using (var httpClient = new HttpClient())
			{
				string encodedOrigin = Uri.EscapeDataString(origin);
				string encodedDestination = Uri.EscapeDataString(destination);
				string requestUrl = $"https://dev.virtualearth.net/REST/v1/Routes?wayPoint.1={encodedOrigin}&wayPoint.2={encodedDestination}&routeAttributes=routeSummariesOnly&key=" + (KEY == null ? GetKey() : KEY);

				HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
          
				response.EnsureSuccessStatusCode();

				var json = await response.Content.ReadAsStringAsync();
				var result = Newtonsoft.Json.JsonConvert.DeserializeObject<BingMapsApiResponse>(json);
				// Extract the distance value from the response
				double distance = result?.ResourceSets?.FirstOrDefault()?.Resources?.FirstOrDefault()?.TravelDistance ?? 0;
                Console.WriteLine(distance);
				return distance;
			}

		}

        public async Task<LocationCollection> GetRoute(List<Location> locations)
        {
            using (var httpClient = new HttpClient())
            {
				string URL = "https://dev.virtualearth.net/REST/v1/Routes?";
				for (int i = 0; i < locations.Count; i++)
				{
					var location = locations[i];
					URL += "waypoint." + i + "=" + location.Latitude + "," + location.Longitude + "&";
				}
				URL += "routeAttributes=routePath&key=" + (KEY == null ? GetKey() : KEY);

                HttpResponseMessage response = await httpClient.GetAsync(URL);
                
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
				dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

				var routePath = new LocationCollection();

				if (jsonResponse?.resourceSets?[0]?.resources?[0]?.routeLegs?[0]?.itineraryItems != null)
				{
					foreach (var itineraryItem in jsonResponse.resourceSets[0].resources[0].routeLegs[0].itineraryItems)
					{
						if (itineraryItem?.point?.coordinates != null && itineraryItem.point.coordinates.Count >= 2)
						{
							var latitude = (double)itineraryItem.point.coordinates[0];
							var longitude = (double)itineraryItem.point.coordinates[1];
							routePath.Add(new Location(latitude, longitude));
						}
					}
				}

				return routePath;

			}
            

		}

        public GeocodeResponse Geocode(string addressQuery)
        {
            string URL = "http://dev.virtualearth.net/REST/v1/Locations/" + addressQuery + "?o=xml&key=" + (KEY == null ? GetKey() : KEY);
            XmlDocument geocodeDocument = GetXmlResponse(URL);
            
            if(geocodeDocument == null) return null;

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(geocodeDocument.NameTable);
            namespaceManager.AddNamespace("ns", "http://schemas.microsoft.com/search/local/ws/rest/v1");

            XmlNode locationNode = geocodeDocument.SelectSingleNode("//ns:Location", namespaceManager);
            if (locationNode != null)
            {
                string country = locationNode.SelectSingleNode("ns:Address/ns:CountryRegion", namespaceManager)?.InnerText;

                if (country != null && country.Equals("Serbia", StringComparison.OrdinalIgnoreCase))
                {
                    string address = locationNode.SelectSingleNode("ns:Address/ns:FormattedAddress", namespaceManager)?.InnerText;
                    string latitudeString = locationNode.SelectSingleNode("ns:Point/ns:Latitude", namespaceManager)?.InnerText;
                    string longitudeString = locationNode.SelectSingleNode("ns:Point/ns:Longitude", namespaceManager)?.InnerText;
                    if (Double.TryParse(latitudeString, out double parsedLatitude) && Double.TryParse(longitudeString, out double parsedLongitude))
                        return new GeocodeResponse { AdressFormatted = address, Latitude = parsedLatitude, Longitude = parsedLongitude };
                }
            }
            return null;
        }

        public GeocodeResponse ReverseGeocode(double latitude, double longitude)
        {
            string URL = "http://dev.virtualearth.net/REST/v1/Locations/" + latitude + "," + longitude + "?o=xml&key=" + (KEY == null ? GetKey() : KEY);
            XmlDocument geocodeDocument = GetXmlResponse(URL);

            if (geocodeDocument == null) return null;

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(geocodeDocument.NameTable);
            namespaceManager.AddNamespace("ns", "http://schemas.microsoft.com/search/local/ws/rest/v1");

            XmlNode locationNode = geocodeDocument.SelectSingleNode("//ns:Location", namespaceManager);
            if (locationNode != null)
            {
                string country = locationNode.SelectSingleNode("ns:Address/ns:CountryRegion", namespaceManager)?.InnerText;
                if (country != null && country.Equals("Serbia", StringComparison.OrdinalIgnoreCase))
                {
                    string address = locationNode.SelectSingleNode("ns:Address/ns:FormattedAddress", namespaceManager)?.InnerText;
                    string latitudeString = locationNode.SelectSingleNode("ns:Point/ns:Latitude", namespaceManager)?.InnerText;
                    string longitudeString = locationNode.SelectSingleNode("ns:Point/ns:Longitude", namespaceManager)?.InnerText;
                    if (Double.TryParse(latitudeString, out double parsedLatitude) && Double.TryParse(longitudeString, out double parsedLongitude))
                        return new GeocodeResponse { AdressFormatted = address, Latitude = parsedLatitude, Longitude = parsedLongitude };
                }
            }
            return null;
        }

        private XmlDocument GetXmlResponse(string requestUrl)
        {
            Console.WriteLine("Request URL (XML): " + requestUrl);
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).",
                    response.StatusCode,
                    response.StatusDescription));
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(response.GetResponseStream());
                    return xmlDoc;
                } catch { return null; }    
            }
        }
    }

    public class GeocodeResponse
    {
        public string AdressFormatted { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

	public class BingMapsApiResponse
	{
		public BingMapsResourceSet[] ResourceSets { get; set; }
	}

	public class BingMapsResourceSet
	{
		public BingMapsResource[] Resources { get; set; }
	}

	public class BingMapsResource
	{
		public double TravelDistance { get; set; }
	}
}
