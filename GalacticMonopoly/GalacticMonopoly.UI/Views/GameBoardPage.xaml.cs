using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using GalacticMonopoly.Core.Enums;
using GalacticMonopoly.Core.Game;
using GalacticMonopoly.Core.Models;
using Controls;

namespace GalacticMonopoly.UI.Views
{
    public partial class GameBoardPage : Page
    {
        private readonly Game _game;
        private bool _hasPromptedThisTurn = false;

        public GameBoardPage(Game game)
        {
            InitializeComponent();
            _game = game;
            BuildBoard();
            ShowPlayers();
            HighlightCurrentPosition();
            UpdateTurnUI();
        }

        private void BuildBoard()
        {
            foreach (var f in _game.State.GalaxyMap.Fields)
            {
                if (f.Type == FieldType.Planet && f.Planet != null)
                {
                    var ctrl = new PlanetFieldControl
                    {
                        PlanetName = f.Planet.Name,
                        BottomInfo = $"Cena: {f.Planet.Price}",
                        BackgroundImage = new BitmapImage(new Uri($"/Images/{f.Planet.Name}.png", UriKind.Relative)),
                        BackgroundColor = Brushes.Black,
                        Width = 70,
                        Height = 120
                    };
                    BoardPanel.Children.Add(ctrl);
                }
                else
                {
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
            var colors = new[] { Brushes.Red, Brushes.Green, Brushes.Blue, Brushes.Orange };

            for (int i = 0; i < _game.State.Players.Count && i < 4; i++)
            {
                var pl = _game.State.Players[i];
                var panel = slots[i];

                var avatar = new Image
                {
                    Source = new BitmapImage(new Uri(pl.AvatarPath, UriKind.Absolute)),
                    Width = 64,
                    Height = 64
                };

                var bordered = new Border
                {
                    BorderBrush = colors[i],
                    BorderThickness = new Thickness(3),
                    Margin = new Thickness(4),
                    Child = avatar
                };

                panel.Children.Add(bordered);
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
            var pl = _game.State.CurrentPlayer;
            TurnText.Text = $"Tura: {pl.Name}";
        }

        private void RollDice_Click(object sender, RoutedEventArgs e)
        {
            var steps = _game.PerformMovement();
            if (steps == -1)
            {
                _game.EndTurn();
                UpdateTurnUI();
                return;
            }

            DiceText.Text = $"Wynik: {steps}";
            _hasPromptedThisTurn = false;
            HighlightCurrentPosition();
            UpdateTurnUI();

            Dispatcher.InvokeAsync(() =>
            {
                _game.ApplyFieldEffects();
                HandlePlanetInteraction();

                // KONIEC TURY po interakcji
                _game.EndTurn();
                UpdateTurnUI();
            }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }


        private void HandlePlanetInteraction()
        {
            if (_hasPromptedThisTurn) return;

            var currentPlayer = _game.State.CurrentPlayer;
            var currentField = _game.State.GalaxyMap.Fields[currentPlayer.Position];

            if (currentField.Type == FieldType.Planet && currentField.Planet != null)
            {
                var planet = currentField.Planet;

                if (planet.IsOwned && planet.Owner == currentPlayer)
                {
                    var result = MessageBox.Show($"Jesteś właścicielem {planet.Name}. Chcesz rozbudować strukturę?", "Rozbudowa", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        MessageBox.Show("(Tu należy dodać logikę rozbudowy)", "Rozbudowa w trakcie implementacji");
                    }
                }
                else if (!planet.IsOwned)
                {
                    var result = MessageBox.Show($"{planet.Name} kosztuje {planet.Price} kredytów. Chcesz kupić?", "Zakup planety", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        if (currentPlayer.Credits >= planet.Price)
                        {
                            currentPlayer.Credits -= planet.Price;
                            planet.Owner = currentPlayer;
                            currentPlayer.OwnedPlanets.Add(planet);
                            MessageBox.Show($"Zakupiono planetę {planet.Name}!", "Sukces");
                        }
                        else
                        {
                            MessageBox.Show("Nie masz wystarczającej liczby kredytów.", "Błąd");
                        }
                    }
                }
            }

            _hasPromptedThisTurn = true;
        }

        private void HighlightCurrentPosition()
        {
            foreach (var child in BoardPanel.Children)
            {
                if (child is FieldControl fc)
                {
                    fc.Occupants = new List<UIElement>();
                    fc.TopMarkers = new List<UIElement>();
                }
                else if (child is PlanetFieldControl pfc)
                {
                    pfc.Occupants = new List<UIElement>();
                    pfc.TopMarkers = new List<UIElement>();
                }
            }

            var colors = new SolidColorBrush[] { Brushes.Red, Brushes.Green, Brushes.Blue, Brushes.Orange };
            var fieldOccupants = new Dictionary<int, List<UIElement>>();

            for (int i = 0; i < _game.State.Players.Count; i++)
            {
                var player = _game.State.Players[i];
                var color = colors[i % colors.Length];

                var circle = new Ellipse
                {
                    Width = 16,
                    Height = 16,
                    Fill = color,
                    Stroke = Brushes.White,
                    StrokeThickness = 1,
                    Margin = new Thickness(1)
                };
                Panel.SetZIndex(circle, 10);

                int pos = player.Position;
                if (!fieldOccupants.ContainsKey(pos))
                {
                    fieldOccupants[pos] = new List<UIElement>();
                }
                fieldOccupants[pos].Add(circle);
            }

            foreach (var kvp in fieldOccupants)
            {
                int pos = kvp.Key;
                var elements = kvp.Value;
                var field = BoardPanel.Children[pos];

                if (field is FieldControl fc)
                {
                    fc.Occupants = elements;
                }
                else if (field is PlanetFieldControl pfc)
                {
                    pfc.Occupants = elements;
                }
            }

            var corners = new[] { TL, TR, BL, BR };
            for (int i = 0; i < _game.State.Players.Count && i < 4; i++)
            {
                var player = _game.State.Players[i];
                var panel = corners[i];
                var color = colors[i % colors.Length];

                for (int j = 0; j < panel.Children.Count; j++)
                {
                    if (panel.Children[j] is Border border && border.Child is Image img)
                    {
                        border.BorderBrush = color;
                    }
                    else if (panel.Children[j] is Image img2)
                    {
                        var newBorder = new Border
                        {
                            BorderBrush = color,
                            BorderThickness = new Thickness(3),
                            Margin = new Thickness(4),
                            Child = img2
                        };
                        panel.Children[j] = newBorder;
                    }
                    else if (panel.Children[j] is TextBlock tb && tb.Text.StartsWith("Kredyty"))
                    {
                        tb.Text = $"Kredyty: {player.Credits}";
                    }
                }
            }

            Console.WriteLine($"Ustawiono kolorowe znaczniki na planszy dla {_game.State.Players.Count} graczy.");
        }


    }
}
