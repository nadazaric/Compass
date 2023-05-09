using Microsoft.Win32;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using travel_agent.Models;
using static travel_agent.Models.Place;
using travel_agent.Services;

namespace travel_agent.WindowsAndPages
{
    public partial class AddPlacePage : Page
    {
        private new readonly MainWindow Parent;
        private PlaceService PlaceService;
        private BitmapImage Image = null;
        public AddPlacePage(MainWindow parent)
        {
            InitializeComponent();
            Parent = parent;
            PlaceService = PlaceService.Instance;
        }

        private void OnAddPictureClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpeg;*.jpg;*.gif;*.bmp)|*.png;*.jpeg;*.jpg;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = openFileDialog.FileName;
                Image = new BitmapImage(new Uri(selectedImagePath));
                var imageBrush = new ImageBrush(Image);
                imageBrush.Stretch = Stretch.UniformToFill;
                imageBrush.AlignmentX = AlignmentX.Center;
                imageBrush.AlignmentY = AlignmentY.Center;
                PlaceImage.Background = imageBrush;
                PlaceImageLabel.Visibility = Visibility.Hidden;
                (sender as Button).Content = "Promeni fotografiju";
            }
        }

        private void OnSubmitClick(object sender, RoutedEventArgs e)
        {
            if (!IsFormInputsValid()) return;
            Place place = new Place
            {
                Name = PlacesNemeInput.InputText,
                Image = Image,
                Description = PlaceDescriptionInput.InputText,
                Type = GetType(),
                Address = PlaceAddressInput.InputText,
                Latitude = 0,
                Longitude = 0,
            };
            PlaceService.Create(place);
        }

        private new PlaceType GetType()
        {
            if (PlaceAtractionRadioBtn.IsChecked == true) return PlaceType.ATRACTION;
            if (PlaceRestaurantRadioBtn.IsChecked == true) return PlaceType.RESTAURANT;
            else return PlaceType.ACCOMMODATION;
        }

        private bool IsPictureValid()
        {
            bool isValid = true;
            if (Image == null)
            {
                PlaceImageError.Visibility = Visibility.Visible;
                isValid = false;
            }
            else PlaceImageError.Visibility = Visibility.Collapsed;
            return isValid;
        }

        private bool IsFormInputsValid()
        {
            bool isAllValid = true;
            if (!PlacesNemeInput.IsValid()) isAllValid = false;
            if (!IsPictureValid()) isAllValid = false;
            if (!PlaceDescriptionInput.IsValid()) isAllValid = false;
            if (!PlaceAddressInput.IsValid()) isAllValid = false;
            return isAllValid;
        }
    }
}
