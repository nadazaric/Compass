﻿using Microsoft.Win32;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using travel_agent.Models;
using static travel_agent.Models.Place;
using travel_agent.Services;
using System.Diagnostics;
using System.Net;

namespace travel_agent.WindowsAndPages
{
    public partial class AddAndModifyPlacePage : Page
    {
        private new readonly MainWindow Parent;
        private Application App;
        private PlaceService PlaceService;
        private BitmapImage Image = null;
        private Place Place {  get; set; }
        public AddAndModifyPlacePage(MainWindow parent, Place place = null)
        {
            InitializeComponent();
            App = Application.Current;
            Parent = parent;
            PlaceService = PlaceService.Instance;
            Place = place;
            SetupPage();
        }

        private void SetupPage()
        {
            if (Place == null) return;
            PlacesNameInput.InputText = Place.Name;
            SetImage(Place.Image);
            PlaceDescriptionInput.InputText = Place.Description;
            SetRadioBtnType();
            PlaceAddressInput.InputText = Place.Address;
            PlacesAddOrModifyButton.Content = "Završi izmenu mesta";
        }

        private void SetImage(BitmapImage image)
        {
            Image = image;
            var imageBrush = new ImageBrush(Image);
            imageBrush.Stretch = Stretch.UniformToFill;
            imageBrush.AlignmentX = AlignmentX.Center;
            imageBrush.AlignmentY = AlignmentY.Center;
            PlaceImage.Background = imageBrush;
            PlaceImageLabel.Visibility = Visibility.Hidden;
            PlaceAddImageButton.Content = "Promeni fotografiju";
        }

        private void SetRadioBtnType()
        {
            if(Place.Type == PlaceType.ATRACTION) PlaceAtractionRadioBtn.IsChecked = true;
            else if(Place.Type == PlaceType.RESTAURANT) PlaceRestaurantRadioBtn.IsChecked = true;
            else PlaceAccommodationRadioBtn.IsChecked = true;
        }

        private void OnAddPictureClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpeg;*.jpg;*.gif;*.bmp)|*.png;*.jpeg;*.jpg;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = openFileDialog.FileName;
                SetImage(new BitmapImage(new Uri(selectedImagePath)));
            }
        }

        private void OnSubmitClick(object sender, RoutedEventArgs e)
        {
            if (!IsFormInputsValid()) return;
            if (Place == null) AddPlace();
            else ModifyPlace();
        }

        private void AddPlace()
        {
            var result = MessageBox.Show("Da li ste sigurni da želite dodati novo mesto?", App.Resources["String.AppName"] as string, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;
            Place = new Place();
            SetPlace();
            PlaceService.Create(Place);
            Parent.MainFrame.Content = new PlacesPage(Parent);
        }

        private void ModifyPlace()
        {
            var result = MessageBox.Show("Da li ste sigurni da želite promeniti mesto?", App.Resources["String.AppName"] as string, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;
            SetPlace();
            PlaceService.Modify(Place);
            Parent.MainFrame.Content = new PlacesPage(Parent);
        }

        private void SetPlace()
        {
            Place.Name = PlacesNameInput.InputText;
            Place.Image = Image;
            Place.Description = PlaceDescriptionInput.InputText;
            Place.Type = GetType();
            Place.Address = PlaceAddressInput.InputText;
            Place.Latitude = 0;
            Place.Longitude = 0;
        }

        private new PlaceType GetType()
        {
            if (PlaceAtractionRadioBtn.IsChecked == true) return PlaceType.ATRACTION;
            else if (PlaceRestaurantRadioBtn.IsChecked == true) return PlaceType.RESTAURANT;
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
            if (!PlacesNameInput.IsValid()) isAllValid = false;
            if (!IsPictureValid()) isAllValid = false;
            if (!PlaceDescriptionInput.IsValid()) isAllValid = false;
            if (!PlaceAddressInput.IsValid()) isAllValid = false;
            return isAllValid;
        }
    }
}