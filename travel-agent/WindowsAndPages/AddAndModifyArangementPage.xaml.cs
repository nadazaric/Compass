using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using travel_agent.Models;
using travel_agent.Services;

namespace travel_agent.WindowsAndPages
{
	/// <summary>
	/// Interaction logic for AddAndModifyArangementPage.xaml
	/// </summary>
	public partial class AddAndModifyArangementPage : Page
	{
		private new readonly MainWindow Parent;
		private Application App;
		private ArrangementService ArrangementService;
		private PlaceService PlaceService;
		private BitmapImage Image = null;
		private Arrangement Arrangement;

		public AddAndModifyArangementPage(MainWindow parent, Arrangement arrangement = null)
		{
			InitializeComponent();
			App = Application.Current;
			Parent = parent;
			ArrangementService = ArrangementService.Instance;
			PlaceService = PlaceService.Instance;
			Arrangement = arrangement;
			SetUpPage();

		}

		private List<Place> FilterPlaces(Place.PlaceType placeType)
		{
			List<Place> allPlaces = PlaceService.GetAllByType(placeType);
			if(Arrangement == null) return allPlaces;
			List<Place> places = (from place in allPlaces where !Arrangement.Places.Contains(place) select place ).ToList();
			return places;
		}

		private List<Place> FilterByName(Place.PlaceType placeType, string name)
		{
			return FilterPlaces(placeType).Where(p => p.Name.ToLower().StartsWith(name.ToLower())).ToList();
		}

		private void SetUpPage()
		{
			List<Place> attractions = this.FilterPlaces(Place.PlaceType.ATRACTION);
			List<Place> restaurants = this.FilterPlaces(Place.PlaceType.RESTAURANT);
			List<Place> accommodation = this.FilterPlaces(Place.PlaceType.ACCOMMODATION);

			if (attractions.Count == 0) {
				AttractionsList.Visibility = Visibility.Collapsed;
				NoContentAttraction.Visibility = Visibility.Visible;
			}
			else AttractionsList.ItemsSource = attractions;
			if(restaurants.Count == 0)
			{
				RestaurantsList.Visibility = Visibility.Collapsed;
				NoContentRestaurants.Visibility = Visibility.Visible;
			}
			else RestaurantsList.ItemsSource = restaurants;
			if(accommodation.Count == 0)
			{
				AccommodationList.Visibility = Visibility.Collapsed;
				NoContentAccommodation.Visibility = Visibility.Visible;
			}
			else AccommodationList.ItemsSource = accommodation;
			if (Arrangement == null) return;
			ArrangementNameInput.InputText = Arrangement.Name;
			SetImage(Arrangement.Image);
			StartDatePicker.SelectedDate = Arrangement.Start;
			EndDatePicker.SelectedDate = Arrangement.End;
			
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
			PlaceAddImageButton.Content = App.Resources["String.SwichImageButton"] as string;
		}

		private void OnAddPictureClick(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Image Files (*.png;*.jpeg;*.jpg;*.gif;*.bmp)|*.png;*.jpeg;*.jpg;*.gif;*.bmp";
			if (openFileDialog.ShowDialog() == true) SetImage(new BitmapImage(new Uri(openFileDialog.FileName)));
		}


		private void searchPlaceInput_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			TextBox innerTextBox = (TextBox)textBox.Template.FindName("PART_TextBox", textBox);
			string textSearch = innerTextBox.Text;
			switch((tabControl.SelectedItem as TabItem).Header)
			{
				case "Atrakcije":
					List<Place> attractions = FilterByName(Place.PlaceType.ATRACTION, textSearch);
					if (attractions.Count == 0)
					{
						AttractionsList.Visibility = Visibility.Collapsed;
						NoContentAttraction.Visibility = Visibility.Visible;
					}
					else {
						AttractionsList.Visibility = Visibility.Visible;
						NoContentAttraction.Visibility = Visibility.Collapsed;
						AttractionsList.ItemsSource = attractions;
					}
					break;
				case "Restorani":
					List<Place>restaurants = FilterByName(Place.PlaceType.RESTAURANT, textSearch);
					if (restaurants.Count == 0)
					{
						RestaurantsList.Visibility = Visibility.Collapsed;
						NoContentRestaurants.Visibility = Visibility.Visible;
					}
					else { RestaurantsList.ItemsSource = restaurants;
						RestaurantsList.Visibility = Visibility.Visible;
						NoContentRestaurants.Visibility = Visibility.Collapsed;
					}
					break;
				case "Smeštaj":
					List<Place> accommodation = FilterByName(Place.PlaceType.ACCOMMODATION, textSearch);
					if (accommodation.Count == 0)
					{
						AccommodationList.Visibility = Visibility.Collapsed;
						NoContentAccommodation.Visibility = Visibility.Visible;
					}
					else { AccommodationList.ItemsSource = accommodation;
						AccommodationList.Visibility = Visibility.Visible;
						NoContentAccommodation.Visibility = Visibility.Collapsed;
					}
					break;
			}
		}

		private void OnBackClick(object sender, RoutedEventArgs e) => Parent.MainFrame.Content = new PlacesPage(Parent);

	}
}
