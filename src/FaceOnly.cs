using System;
using System.Windows;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using System.ComponentModel;
using System.Windows.Data;
using HearthDb.Enums;
using System.Linq;

namespace HDT.Plugins.FaceOnly
{
	public class FaceOnly : IPlugin
	{
		private Mask mask;
		private float opacity = 1f;
		private String colorHex = "#32302f";
		private bool gameStarted = false;
		private bool mulliganDone = false;

		#region Manifest
		public string Author
		{
			get { return "Anthony Fu"; }
		}

		public string ButtonText
		{
			get { return "Settings"; }
		}

		public string Description
		{
			get { return "A toy for face decks."; }
		}

		public string Name
		{
			get { return "FaceOnly"; }
		}

		public Version Version
		{
			get { return new Version(0, 0, 4); }
		}
		#endregion

		public MenuItem MenuItem
		{
			get { return null; }
		}

		public void OnButtonPress()
		{
		}

		public void OnLoad()
		{
			mask = new Mask();
			mask.Opacity = opacity;
			mask.Color = colorHex;
			Core.OverlayCanvas.Children.Add(mask);

			//events													 
			GameEvents.OnGameStart.Add(() =>
			{
				gameStarted = true;
			});
			GameEvents.OnTurnStart.Add(a => mask.TurnStart());
			GameEvents.OnGameEnd.Add(() =>
			{
				gameStarted = false;
				mask.GameEnd();
			});
   
			Canvas.SetTop(mask, -2);
			Canvas.SetLeft(mask, 0);
			Canvas.SetBottom(mask, 0);
			Canvas.SetRight(mask, 0);
			Canvas.SetZIndex(mask, -1);

			Binding widthBinding = new Binding();
			widthBinding.Mode = BindingMode.OneWay;
			widthBinding.Source = Core.OverlayCanvas.ActualWidth;
			mask.SetBinding(Canvas.WidthProperty, widthBinding);

			Binding heightBinding = new Binding();
			heightBinding.Mode = BindingMode.OneWay;
			heightBinding.Source = Core.OverlayCanvas.ActualHeight;
			mask.SetBinding(Canvas.HeightProperty, heightBinding);

			Core.OverlayCanvas.SizeChanged += (s, e) =>
			{
				var size = e.NewSize;
				mask.Height = size.Height;
				mask.Width = size.Width;
			};

			mulliganDone = Core.Game.IsMulliganDone;
			gameStarted = mulliganDone && !Core.Game.IsInMenu;

			if (!mulliganDone)
			{
				mask.GameEnd();
			}
			else
			{
				mask.GameStart();
			}
		}

		public void OnUnload()
		{
			Core.OverlayCanvas.Children.Remove(mask);
		}

		public void OnUpdate()
		{
			if (gameStarted)
			{
				if (!mulliganDone && Core.Game.IsMulliganDone)
					mask.GameStart();
				mulliganDone = Core.Game.IsMulliganDone;

				if (mask.Masked){
					mask.CountChanged(Core.Game.OpponentMinionCount);

					var oppBoard = Core.Game.Opponent.Board.Where(x => x.IsMinion).OrderBy(x => x.GetTag(GameTag.ZONE_POSITION)).ToList();

					int i = 0;
					foreach (var e in oppBoard)
					{
						if (e.IsInPlay) {
							if (e.GetTag(GameTag.TAUNT) == 1)
								mask.SetTaunt(i);
							else
								mask.Reset(i);
							i++;
						}
					}
				}			

				mask.OnUpdate();
			}
		}
	}
}