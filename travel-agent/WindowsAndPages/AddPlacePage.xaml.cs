using Microsoft.Win32;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace travel_agent.WindowsAndPages
{
    public partial class AddPlacePage : Page
    {
        private new readonly MainWindow Parent;
        public AddPlacePage(MainWindow parent)
        {
            InitializeComponent();
            Parent = parent;
        }

        private void OnAddPictureClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpeg;*.jpg;*.gif;*.bmp)|*.png;*.jpeg;*.jpg;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = openFileDialog.FileName;
                BitmapImage image = new BitmapImage(new Uri(selectedImagePath));
                var imageBrush = new ImageBrush(image);
                imageBrush.Stretch = Stretch.UniformToFill;
                imageBrush.AlignmentX = AlignmentX.Center;
                imageBrush.AlignmentY = AlignmentY.Center;
                PlaceImage.Background = imageBrush;
                PlaceImageLabel.Visibility = Visibility.Hidden;
                (sender as Button).Content = "Promeni fotografiju";
            }
        }

    }
}
