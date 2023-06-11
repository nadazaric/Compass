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

namespace travel_agent.WindowsAndPages.Reports
{
    /// <summary>
    /// Interaction logic for ReportForArrangement.xaml
    /// </summary>
    public partial class ReportForArrangement : Page
    {
        private new readonly MainWindow Parent;
        private Application App;
        public ReportForArrangement(MainWindow parent)
        {
            InitializeComponent();
            Parent = parent;
            App = Application.Current;
            DataContext = this;
        }
    }
}
