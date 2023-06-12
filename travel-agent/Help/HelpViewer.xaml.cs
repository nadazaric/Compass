using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using travel_agent.WindowsAndPages;
using Path = System.IO.Path;

namespace travel_agent.Help
{
	/// <summary>
	/// Interaction logic for HelpViewer.xaml
	/// </summary>
	public partial class HelpViewer : Window
	{
		private JavaScriptControlHelp ch;
		public HelpViewer(string key, MainWindow originator)
		{
			InitializeComponent();
			string executablePath = Assembly.GetExecutingAssembly().Location;
			string rootPath = Path.GetDirectoryName(executablePath);

			string curDir = rootPath.Substring(0, rootPath.Length - 10) + "/Help/Files/";
			Console.WriteLine(curDir);
			string path = curDir + key + ".htm";
			if (!File.Exists(path))
			{
				key = "error";
			}
			Uri u = new Uri(curDir+key+".htm");
			//System.Diagnostics.Process.Start(u.ToString());
			ch = new JavaScriptControlHelp(originator);
			wbHelp.ObjectForScripting = ch;
			wbHelp.Navigate(u);

		}

		private void BrowseBack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = ((wbHelp != null) && (wbHelp.CanGoBack));
		}

		private void BrowseBack_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			wbHelp.GoBack();
		}

		private void BrowseForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = ((wbHelp != null) && (wbHelp.CanGoForward));
		}

		private void BrowseForward_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			wbHelp.GoForward();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
		}

		private void wbHelp_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
		{
		}

	}
}
