﻿using System;
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

namespace travel_agent.WindowsAndPages
{
	/// <summary>
	/// Interaction logic for ViewArrangementPage.xaml
	/// </summary>
	public partial class ViewArrangementPage : Page
	{

		private Arrangement arrangement;
		private MainWindow parent;

		public ViewArrangementPage(MainWindow parent, Arrangement arrangement)
		{
			InitializeComponent();
			this.parent = parent;
			this.arrangement = arrangement;
			DataContext = this.arrangement;
		}

		private void OnBackClick(object sender, RoutedEventArgs e) => parent.MainFrame.Content = new ArrangementsPage(parent);
	}
}
