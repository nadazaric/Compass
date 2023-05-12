using System;
using System.IO;
using System.Net;
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
}
