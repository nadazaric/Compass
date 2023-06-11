using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using travel_agent.Models;

namespace travel_agent.Controls
{
    public partial class FancyMap : UserControl
    {
        public Pushpin Pin { get; set; } = null;
        public List<Pushpin> AllPins { get; set; } = new List<Pushpin>();
        public List<MapPolyline> Polylines { get; set; } = new List<MapPolyline> ();
        public GeocodeResponse LastGeocodeResponse { get; set; }
        private Geocoder Geocoder;
        private static int ZOOM_LEVEL = 13;
        private bool IsDoubleClickDisabled = false;
        private Application App;
        public FancyMap()
        {
            InitializeComponent();
            Geocoder = Geocoder.Instance;
            App = Application.Current;
            map.CredentialsProvider = new ApplicationIdCredentialsProvider(GetKey());
            map.Center = new Location(45.256016757384884, 19.840603063313143);
            map.ZoomLevel = ZOOM_LEVEL;
            Loaded += AfterLoad;
        }

        private void AfterLoad(object sender, RoutedEventArgs e)
        {
            var rect = new RectangleGeometry(new Rect(0, 0, ActualWidth - 2, ActualHeight - 2));
            rect.RadiusX = 6;
            rect.RadiusY = 6;
            map.Clip = rect;
        }

        private string GetKey()
        {
            try { return File.ReadAllText("../../mapkey.txt"); }
            catch (FileNotFoundException) { return string.Empty; }
        }

        private void OnDrawPinDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (IsDoubleClickDisabled) return;
            e.Handled = true;
            System.Windows.Point mousePosition = e.GetPosition(this);
            Location location = map.ViewportPointToLocation(mousePosition);
            DrawPin(location);
            LastGeocodeResponse = Geocoder.ReverseGeocode(location.Latitude, location.Longitude);
            OnPinPlaced(LastGeocodeResponse == null ? null : LastGeocodeResponse.AdressFormatted);

        }

        public void DisableDoubleClick() => IsDoubleClickDisabled = true;

        public void ManuallySetInitialState(GeocodeResponse location)
        {
            LastGeocodeResponse = location;
            DrawPin(new Location(location.Latitude, location.Longitude));
        }

        public void DrawPin(Location location)
        {
            if (Pin != null) map.Children.Remove(Pin);
            Pin = new Pushpin();
            Pin.Location = location;
            Pin.Background = App.Resources["Color.PrimaryDark"] as SolidColorBrush;
            map.Children.Add(Pin);
            map.Center = location;
            map.ZoomLevel = ZOOM_LEVEL;
        }

        public void UpdatePinsContent()
        {
            foreach(var pin in AllPins)
            {
				pin.Content = AllPins.IndexOf(pin) + 1;
			}
		}

        public void DrawPinForRoute(Place place, int index = -1)
        {
            Location location = new Location(place.Latitude, place.Longitude);
			Pin = new Pushpin();
			Pin.Location = location;
			Pin.Background = App.Resources["Color.PrimaryDark"] as SolidColorBrush;
			map.Children.Add(Pin);
			map.Center = location;
			map.ZoomLevel = ZOOM_LEVEL;
			Pin.ToolTip = new ToolTip { Content = place.Name };
			if (index != -1)
            {
                DeletePin(location);
                AllPins.Insert(index, Pin);
            }
            else
            {
                DeletePin(location);
                AllPins.Add(Pin);
            }
            UpdatePinsContent();
		}

        public async void DrawRouteAsync(List<ArrangementStep> steps)
        {
            DeleteRoutes();
            foreach(var step in steps)
            {
				MapPolyline routePolyline = new MapPolyline();
				if (step.TransportationType != ArrangementStep.TransportType.PLANE)
                {
					LocationCollection routePoints = await Geocoder.GetRoute(step);
					routePolyline.Locations = routePoints;
                }
                else
                {
					LocationCollection routePoints = new LocationCollection
					{
						new Location(step.StartPlace.Latitude, step.StartPlace.Longitude),
						new Location(step.EndPlace.Latitude, step.EndPlace.Longitude)
					};
                    routePolyline.Locations = routePoints;
                }
                
				routePolyline.Stroke = new SolidColorBrush(Color.FromRgb(51, 107, 135));
				routePolyline.StrokeThickness = 3;

                if(step.TransportationType == ArrangementStep.TransportType.FOOT)
                {
                    routePolyline.StrokeDashArray = new DoubleCollection { 4, 1 };

				}
                else if(step.TransportationType == ArrangementStep.TransportType.PLANE)
                {
					routePolyline.Stroke = new SolidColorBrush(Color.FromRgb(86, 141, 166));
				}else if(step.TransportationType == ArrangementStep.TransportType.TRAIN)
                {
					routePolyline.Stroke = new SolidColorBrush(Color.FromRgb(122, 122, 122));
					routePolyline.StrokeDashArray = new DoubleCollection { 2, 1 };
                    routePolyline.ToolTip = new ToolTip { Content = "Nisu dostupni podaci za rutu vozom" };

				}
				map.Children.Add(routePolyline);
				Polylines.Add(routePolyline);
			}
			
			

			
		}

        public void DeletePin(Location location)
        {
            Pushpin toRemove = new Pushpin();
            foreach(var pin in AllPins)
            {
                if(pin.Location == location)
                {
                    map.Children.Remove(pin);
                    toRemove = pin;
                }
            }
            AllPins.Remove(toRemove);
			UpdatePinsContent();
		}

        public void DeleteRoutes()
        {
            foreach(var poly in Polylines)
            {
                map.Children.Remove(poly);
            }
            Polylines.Clear();
        }

		public string TryDrawPinFromAddressLine(string addresQuery)
        {
            LastGeocodeResponse = Geocoder.Geocode(addresQuery);
            if (LastGeocodeResponse == null)
            {
                if (Pin != null) map.Children.Remove(Pin);
                return null;
            }
            DrawPin(new Location(LastGeocodeResponse.Latitude, LastGeocodeResponse.Longitude));
            return LastGeocodeResponse.AdressFormatted;
        }

        public event EventHandler<string> PinPlaced;

        private void OnPinPlaced(string address)
        {
            PinPlaced?.Invoke(this, address);
        }
    }
}
