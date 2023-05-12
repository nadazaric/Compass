using Microsoft.Maps.MapControl.WPF;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace travel_agent.Controls
{
    public partial class FancyMap : UserControl
    {
        private static int ZOOM_LEVEL = 13;
        public Pushpin Pin { get; set; } = null;
        private Geocoder Geocoder;
        public GeocodeResponse LastGeocodeResponse { get; set; }
        public FancyMap()
        {
            InitializeComponent();
            Geocoder = Geocoder.Instance;
            map.CredentialsProvider = new ApplicationIdCredentialsProvider(GetKey());
            map.Center = new Location(45.256016757384884, 19.840603063313143);
            map.ZoomLevel = ZOOM_LEVEL;
        }

        private string GetKey()
        {
            try { return File.ReadAllText("../../mapkey.txt"); }
            catch (FileNotFoundException) { return string.Empty; }
        }

        private void OnDrawPinDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point mousePosition = e.GetPosition(this);
            Location location = map.ViewportPointToLocation(mousePosition);
            DrawPin(location);
            LastGeocodeResponse = Geocoder.ReverseGeocode(location.Latitude, location.Longitude);
            OnPinPlaced(LastGeocodeResponse == null ? null : LastGeocodeResponse.AdressFormatted);

        }

        public void DrawPin(Location location)
        {
            if (Pin != null) map.Children.Remove(Pin);
            Pushpin pin = new Pushpin();
            pin.Location = location;
            pin.Background = Application.Current.Resources["Color.PrimaryDark"] as SolidColorBrush;
            map.Children.Add(pin);
            Pin = pin;
            map.Center = location;
            map.ZoomLevel = ZOOM_LEVEL;
        }

        public string TryDrawPinFromAddressLine(string addresQuery)
        {
            LastGeocodeResponse = Geocoder.Geocode(addresQuery);
            if (LastGeocodeResponse == null) return null;
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
