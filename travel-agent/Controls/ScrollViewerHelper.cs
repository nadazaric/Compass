using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace travel_agent.Controls
{
	public class ScrollViewerHelper
	{
		public static bool GetIsScrollingEnabled(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsScrollingEnabledProperty);
		}

		public static void SetIsScrollingEnabled(DependencyObject obj, bool value)
		{
			obj.SetValue(IsScrollingEnabledProperty, value);
		}

		public static readonly DependencyProperty IsScrollingEnabledProperty =
			DependencyProperty.RegisterAttached(
				"IsScrollingEnabled",
				typeof(bool),
				typeof(ScrollViewerHelper),
				new PropertyMetadata(true, OnIsScrollingEnabledChanged));

		private static void OnIsScrollingEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is UIElement element)
			{
				if ((bool)e.NewValue)
				{
					element.MouseEnter += Element_MouseEnter;
					element.MouseLeave += Element_MouseLeave;
				}
				else
				{
					element.MouseEnter -= Element_MouseEnter;
					element.MouseLeave -= Element_MouseLeave;
				}
			}
		}

		private static void Element_MouseEnter(object sender, MouseEventArgs e)
		{
			if (sender is UIElement element)
			{
				var scrollViewer = FindParentScrollViewer(element);
				if (scrollViewer != null)
				{
					scrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;
				}
			}
		}

		private static void Element_MouseLeave(object sender, MouseEventArgs e)
		{
			if (sender is UIElement element)
			{
				var scrollViewer = FindParentScrollViewer(element);
				if (scrollViewer != null)
				{
					scrollViewer.PreviewMouseWheel -= ScrollViewer_PreviewMouseWheel;
				}
			}
		}

		private static void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			e.Handled = true;
		}

		private static ScrollViewer FindParentScrollViewer(UIElement element)
		{
			DependencyObject parent = VisualTreeHelper.GetParent(element);
			while (parent != null && !(parent is ScrollViewer))
			{
				parent = VisualTreeHelper.GetParent(parent);
			}
			return parent as ScrollViewer;
		}
	}
}
