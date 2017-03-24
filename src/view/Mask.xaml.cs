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
		private SolidColorBrush _color;
		private SolidColorBrush _trans;
		private List<Label> _cards;

		public Mask()
		{
			InitializeComponent();
			Masked = false;
			_cards = new List<Label>(7);
			_cards.Add(card1);
			_cards.Add(card2);
			_cards.Add(card3);
			_cards.Add(card4);
			_cards.Add(card5);
			_cards.Add(card6);
			_cards.Add(card7);
			_trans = new SolidColorBrush(Colors.Transparent);
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
			CountChanged(0);
			ResetTaunt();
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
				_color = new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
				bg.Background = _color;
			}
		}

		public void GameEnd()
		{
			Visibility = Visibility.Hidden;
			Masked = false;
		}

		public void ResetTaunt() {
			foreach(var card in _cards)
				card.Background = _color;
		}

		public void Reset(int position)
		{
			if (position < _cards.Count)
				_cards[position].Background = _color;
		}

		public void CountChanged(int count)
		{
			for (int i = 0; i < 7; i++)
				_cards[i].Visibility = i < count ? Visibility.Visible : Visibility.Collapsed;	
		}

		public void SetTaunt(int position) {
			if (position < _cards.Count)
				_cards[position].Background = null;
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
