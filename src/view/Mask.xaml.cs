using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HDT.Plugins.FaceOnly
{
	/// <summary>
	/// Interaction logic for Mask.xaml
	/// </summary>
	public partial class Mask : UserControl
	{
		private bool _masked = false;
		private bool _defaultStatus = false;
		private bool _hoverOnToggler = false;

		public Mask()
		{
			InitializeComponent();
			Masked = false;
		}

		public bool Masked
		{
			get
			{
				return _masked;
			}
			set
			{
				_masked = value;
				mask.Visibility = _masked ? Visibility.Visible : Visibility.Hidden;
			}
		}

		public void GameStart()
		{
			Visibility = Visibility.Visible;
			Masked = _defaultStatus;
		}

		public void TurnStart()
		{
		}

		public string Color
		{
			set
			{
				bg.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
			}
		}

		public void GameEnd()
		{
			Visibility = Visibility.Hidden;
			Masked = false;
		}

		public void HasTaunt(int position, int count)
		{

		}
		private void Toggle()
		{
			Toggle(null, null);
		}

		private void Toggle(object sender, RoutedEventArgs e)
		{
			Masked = !Masked;
		}

		public void OnUpdate()
		{
			var pos = User32.GetMousePos();
			var p = toggler.PointFromScreen(new Point(pos.X, pos.Y));

			var hovered = p.X > 0 && p.X < toggler.ActualWidth && p.Y > 0 && p.Y < toggler.ActualHeight;
			if (!_hoverOnToggler && hovered)
			{
				Toggle();
			}
			_hoverOnToggler = hovered;
		}
	}
}
