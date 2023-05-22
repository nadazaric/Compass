using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using travel_agent.Models;

namespace travel_agent.WindowsAndPages
{
    public partial class ArrangementsPage : Page
    {
        private new readonly MainWindow Parent;
        private Application App;
        public ObservableCollection<Arrangement> Arrangements { get; set; }
        private List<Arrangement> ArrangementsList { get; set; }
        

        public ArrangementsPage()
        {
            InitializeComponent();
            App = Application.Current;
            DataContext = this;
            if (Arrangements.Count <= 1) SetIfNoContent();
              
        }

  //      private void Search()
		//{
  //          Arrangements = new ObservableCollection<Arrangement>();
  //          if (Parent.User.Role == Role.AGENT) Arrangements.Add(null);
  //          foreach(var arrangement in ArrangementsList)
		//	{
  //              if(arrangement.Name.ToLower().Contains(ArrangementSearchName.InputText.ToLower() && )
		//	}
		//}

        private void OnAddNewArangementClick(object sender, System.Windows.RoutedEventArgs e) => Parent.MainFrame.Content = new AddAndModifyArangementPage(Parent);

        private void OnArrangementItemClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
            object data = (sender as Grid).DataContext;
            if (Parent.User.Role == Models.Role.AGENT) Parent.MainFrame.Content = new AddAndModifyArangementPage(Parent, data as Arrangement);
		}

        private void SetIfNoContent()
		{
            if(Parent.User.Role == Role.AGENT)
			{
                ArrangementsListView.Margin = new Thickness(20);
                ArrangementsSearch.Visibility = Visibility.Collapsed;
			}
			else
			{
                ArrangementsSearch.Visibility=Visibility.Collapsed;
                ArrangementsListView.Visibility = Visibility.Collapsed;
                NoContent.Visibility = Visibility.Visible;

            }
		}

        //private void SetIfNoContentAfterSearch()
        //{
        //    NoContent.Visibility = Visibility.Visible;
        //    PlacesList.Visibility = Visibility.Collapsed;
        //    WasListColapsed = true;
        //}

        //private void SetIfHaveContentAfterSearch()
        //{
        //    NoContent.Visibility = Visibility.Collapsed;
        //    PlacesList.Visibility = Visibility.Visible;
        //    WasListColapsed = false;
        //}

        //private void SearchArrangementOnEnter(object sender, EventArgs e) => Search();

    }
}
