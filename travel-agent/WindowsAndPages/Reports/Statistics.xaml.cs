using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using travel_agent.Controls;
using travel_agent.Models;
using travel_agent.Services;
using travel_agent.WindowsAndPages.Reports;

namespace travel_agent.WindowsAndPages
{
    public partial class Statistics : Page
    {
        private new readonly MainWindow Parent;
        private Application App;
        public Statistics(MainWindow parent)
        {
            InitializeComponent();
            Parent = parent;
            App = Application.Current;
            DataContext = this;
        }


        public void ReportForArrangemnet(object sender, RoutedEventArgs e)
        {
            Parent.MainFrame.Content = new ReportForArrangement(Parent);
        }


        public void MonthReport(object sender, RoutedEventArgs e)
        {
            Parent.MainFrame.Content = new MonthReport(Parent);
        }

        public void OnBackClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
