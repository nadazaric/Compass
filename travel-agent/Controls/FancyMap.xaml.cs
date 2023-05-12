using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Core;
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
            GeocodeResponse response = Geocoder.ReverseGeocode(location.Latitude, location.Longitude);
            OnPinPlaced(response == null ? null : response.AdressFormatted);

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
            GeocodeResponse response = Geocoder.Geocode(addresQuery);
            if (response == null) return null;
            DrawPin(new Location(response.Latitude, response.Longitude));
            return response.AdressFormatted;
        }

        public event EventHandler<string> PinPlaced;

        private void OnPinPlaced(string address)
        {
            PinPlaced?.Invoke(this, address);
        }
    }
}
