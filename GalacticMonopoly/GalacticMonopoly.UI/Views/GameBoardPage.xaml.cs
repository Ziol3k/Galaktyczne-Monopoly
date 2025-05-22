// GalacticMonopoly.UI/Views/GameBoardPage.xaml.cs
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Controls;
using GalacticMonopoly.Core.Enums;
using GalacticMonopoly.Core.Game;
using GalacticMonopoly.Core.Models;
using CoreEnums = GalacticMonopoly.Core.Enums;

namespace GalacticMonopoly.UI.Views
{
    public partial class GameBoardPage : Page
    {
        private readonly Game _game;
        private int _turn;

        public GameBoardPage(Game game)
        {
            InitializeComponent();
            _game = game;
            BuildBoard();
            ShowPlayers();
            UpdateTurnUI();
        }

        private void BuildBoard()
        {
            foreach (var f in _game.State.GalaxyMap.Fields)
            {
                if (f.Type == CoreEnums.FieldType.Planet && f.Planet != null)
                {
                    var p = f.Planet;
                    var ctrl = new PlanetFieldControl
                    {
                        PlanetName = p.Name,
                        BottomInfo = $"Cena: {p.Price}",
                        BackgroundImage = new BitmapImage(new Uri($"/Images/{p.Name}.png", UriKind.Relative)),
                        BackgroundColor = Brushes.Black,
                        Width = 70,
                        Height = 120
                    };
                    BoardPanel.Children.Add(ctrl);
                }
                else
                {
                    // używamy FieldControl
                    var iconPath = f.Type switch
                    {
                        FieldType.Start => "/Images/icons/start.png",
                        FieldType.Space => "/Images/icons/space.png",
                        FieldType.Wormhole => "/Images/icons/wormhole.png",
                        FieldType.BlackHole => "/Images/icons/blackhole.png",
                        FieldType.SingularZone => "/Images/icons/singular.png",
                        FieldType.PirateAttack => "/Images/icons/pirate.png",
                        FieldType.GalacticTrainStop => "/Images/icons/train.png",
                        _ => "/Images/icons/space.png",
                    };
                    var fld = new FieldControl
                    {
                        FieldName = f.Name,
                        IconSource = new BitmapImage(new Uri(iconPath, UriKind.Relative)),
                        Width = 70,
                        Height = 70
                    };
                    BoardPanel.Children.Add(fld);
                }
            }
        }


        private void ShowPlayers()
        {
            var slots = new[] { TL, TR, BL, BR };
            for (int i = 0; i < _game.State.Players.Count && i < 4; i++)
            {
                var pl = _game.State.Players[i];
                var panel = slots[i];

                panel.Children.Add(new Image
                {
                    Source = new BitmapImage(new Uri(pl.AvatarPath, UriKind.Absolute)),
                    Width = 64,
                    Height = 64
                });
                panel.Children.Add(new TextBlock
                {
                    Text = pl.Name,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold
                });
                panel.Children.Add(new TextBlock
                {
                    Text = $"Kredyty: {pl.Credits}",
                    Foreground = Brushes.LightGreen
                });
            }
        }

        private void UpdateTurnUI()
        {
            var pl = _game.State.Players[_turn];
            TurnText.Text = $"Tura: {pl.Name}";
        }

        private void RollDice_Click(object sender, RoutedEventArgs e)
        {
            var r = new Random().Next(1, 7);
            DiceText.Text = $"Wynik: {r}";
            _turn = (_turn + 1) % _game.State.Players.Count;
            UpdateTurnUI();
        }
    }
}
