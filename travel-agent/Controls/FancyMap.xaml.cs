using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Core;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace travel_agent.Controls
{
    public partial class FancyMap : UserControl
    {
        public Pushpin Pin { get; set; } = null;
        public FancyMap()
        {
            InitializeComponent();
            map.CredentialsProvider = new ApplicationIdCredentialsProvider(GetKey());
            map.Center = new Location(45.256016757384884, 19.840603063313143);
            map.ZoomLevel = 13;
        }

        private string GetKey()
        {
            try
            {
                return File.ReadAllText("../../mapkey.txt");
            }
            catch (FileNotFoundException)
            {
                return string.Empty;
            }
        }

        private void OnDrawPinDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Pin != null) map.Children.Remove(Pin);
            e.Handled = true;
            
            Point mousePosition = e.GetPosition(this);
            Location pinLocation = map.ViewportPointToLocation(mousePosition);
            Pushpin pin = new Pushpin();
            pin.Location = pinLocation;
            pin.Background = Application.Current.Resources["Color.PrimaryDark"] as SolidColorBrush;
            map.Children.Add(pin);
            Pin = pin;
        }
    }
}
