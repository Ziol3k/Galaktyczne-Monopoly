using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GalacticMonopoly.Core.Enums;
using GalacticMonopoly.Core.Game;
using GalacticMonopoly.Core.Models;
using Controls;
using GalacticMonopoly.Core.Services;

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
            BoardPanel.Children.Clear();
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
                        Width = 90,
                        Height = 144
                    };
                    BoardPanel.Children.Add(ctrl);
                }
                else
                {
                    var iconPath = f.Type switch
                    {
                        FieldType.Start => "/Images/icons/start.png",
                        FieldType.Space => "/Images/icons/space.png",
                        //FieldType.Wormhole => "/Images/icons/wormhole.png",
                        //FieldType.BlackHole => "/Images/icons/blackhole.png",
                        FieldType.SingularZone => "/Images/icons/singular.png",
                        FieldType.PirateAttack => "/Images/icons/pirate.png",
                        FieldType.GalacticTrainStop => "/Images/icons/train.png",
                        _ => "/Images/icons/space.png",
                    };
                    var fld = new FieldControl
                    {
                        FieldName = f.Name,
                        IconSource = new BitmapImage(new Uri(iconPath, UriKind.Relative)),
                        Width = 90,
                        Height = 144
                    };
                    BoardPanel.Children.Add(fld);
                }
            }
        }

        private void ShowPlayers()
        {
            // Zakładamy, że masz 4 rogi: TL, TR, BL, BR jako StackPanel.
            var slots = new[] { TL, TR, BL, BR };
            var colors = new[] { Brushes.Red, Brushes.Green, Brushes.Blue, Brushes.Orange };

            for (int i = 0; i < _game.State.Players.Count && i < 4; i++)
            {
                var pl = _game.State.Players[i];
                var panel = slots[i];

                panel.Children.Clear();

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

                var assetBtn = new Button
                {
                    Content = "Aktywa",
                    Tag = i,
                    Margin = new Thickness(0, 8, 0, 0),
                    Height = 24,
                    Width = 80,
                    Style = (Style)FindResource("CosmoButton")
                };
                assetBtn.Click += AssetBtn_Click;
                panel.Children.Add(assetBtn);

            }

            UpdateTurnUI();
        }

        private void AssetBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int playerIndex)
            {
                var player = _game.State.Players[playerIndex];
                bool isCurrentPlayer = player == _game.State.CurrentPlayer;
                bool hasUpgradedThisTurn = player.HasUpgradedThisTurn;

                var window = new PlayerAssetsWindow(player, _game.State, isCurrentPlayer, hasUpgradedThisTurn, RefreshUI);
                window.Owner = Window.GetWindow(this);
                window.ShowDialog();
            }
        }

        private void RefreshUI()
        {
            HighlightCurrentPosition();
            UpdateTurnUI();
        }

        private void UpdateTurnUI()
        {
            var pl = _game.State.CurrentPlayer;
            TurnText.Text = $"Tura: {pl.Name}";

            SkipTurnButton.Visibility = _game.State.CurrentPlayer.ConsecutiveSkips < 2 ? Visibility.Visible : Visibility.Collapsed;

        }

        private void SkipTurn_Click(object sender, RoutedEventArgs e)
        {
            var player = _game.State.CurrentPlayer;
            if (player.CanSkipTurn)
            {
                player.SkipTurn();
                _game.EndTurn();
                UpdateTurnUI();
                HighlightCurrentPosition();
                DiceText.Text = "Wynik: -";
            }
            else
            {
                MessageBox.Show("Nie możesz już pominąć więcej tur z rzędu!", "Brak możliwości");
            }
        }


        private void RollDice_Click(object sender, RoutedEventArgs e)
        {
            var steps = _game.PerformMovement();

            // Jeśli gracz miał status SkippedTurn, zakończ turę od razu (tak już masz)
            if (steps == -1)
            {
                // Po pominięciu tury licznik się NIE resetuje
                _game.State.CurrentPlayer.HasUpgradedThisTurn = false;
                _game.EndTurn();
                UpdateTurnUI();
                return;
            }

            // *** TU RESETUJEMY licznik ConsecutiveSkips ***
            _game.State.CurrentPlayer.ConsecutiveSkips = 0;

            DiceText.Text = $"Wynik: {steps}";
            _hasPromptedThisTurn = false;
            HighlightCurrentPosition();
            UpdateTurnUI();

            Dispatcher.InvokeAsync(() =>
            {
                var currentPlayer = _game.State.CurrentPlayer;
                var currentField = _game.State.GalaxyMap.Fields[currentPlayer.Position];

                if (currentField.Type == FieldType.SingularZone)
                {
                    HandleSingularZone(currentPlayer, _game.State);
                }
                else
                {
                    // USUŃ drugą deklarację var currentField = ...
                    if (currentField.Type == FieldType.GalacticTrainStop &&
                        currentPlayer.Cards.Any(c => c.Type == CardType.GalacticTicket))
                    {
                        HandleGalacticTrainStop(currentPlayer, _game.State);
                    }
                    else
                    {
                        _game.ApplyFieldEffects();
                        HandlePlanetInteraction();
                    }
                }

                _game.State.CurrentPlayer.HasUpgradedThisTurn = false;
                _game.EndTurn();
                UpdateTurnUI();
            }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);


        }

        private void HandleGalacticTrainStop(Player player, GameState state)
        {
            var ticketCard = player.Cards.FirstOrDefault(c => c.Type == CardType.GalacticTicket);
            if (ticketCard == null)
                return;

            // Wszystkie stacje oprócz bieżącej
            var allStations = state.GalaxyMap.Fields
                .Select((f, i) => new { Field = f, Index = i })
                .Where(x => x.Field.Type == FieldType.GalacticTrainStop && x.Index != player.Position)
                .Select(x => (x.Index, x.Field.Name))
                .ToList();

            if (!allStations.Any())
            {
                MessageBox.Show("Brak innych stacji do wyboru.", "Galaktyczny bilet", MessageBoxButton.OK);
                return;
            }

            var win = new GalacticTicketWindow(allStations)
            {
                Owner = Window.GetWindow(this),
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            if (win.ShowDialog() == true && win.SelectedStationIndex.HasValue)
            {
                player.Position = win.SelectedStationIndex.Value;
                player.RemoveCard(ticketCard);
                HighlightCurrentPosition();
                MessageBox.Show($"Przeniesiono na stację: {state.GalaxyMap.Fields[player.Position].Name}", "Galaktyczny bilet", MessageBoxButton.OK);
            }
        }


        private void HandleSingularZone(Player player, GameState state)
        {
            // 1. Losuj kartę (ale jeszcze nie rozpatruj!)
            var card = CardServices.DrawCard(state);

            // 2. Przygotuj mapowanie typów kart na pliki obrazków
            var cardImages = new Dictionary<CardType, string>
                {
                    { CardType.PirateAttack, "/Images/cards/PirateAttack.png" },
                    { CardType.PirateDefense, "/Images/cards/PirateDefense.png" },
                    { CardType.GalacticTicket, "/Images/cards/GalacticTicket.png" },
                    { CardType.PropertyTax, "/Images/cards/PropertyTax.png" },
                    { CardType.LotteryWin, "/Images/cards/LotteryWin.png" },
                    { CardType.EngineFailure, "/Images/cards/EngineFailure.png" },
                    { CardType.ShipyardMalfunction, "/Images/cards/ShipyardMalfunction.png" },
                };
            // Lista wszystkich ścieżek kart:
            var allCards = new List<string>(cardImages.Values);

            // 3. Wyświetl animację
            var cardDrawWindow = new CardDrawWindow(allCards, cardImages[card.Type])
            {
                Owner = Window.GetWindow(this),
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            cardDrawWindow.ShowDialog();


            // 4. Dopiero teraz rozpatrz efekt karty
            CardServices.ResolveCard(player, card, state);

            // 5. (opcjonalnie) Pokaż opis karty
            MessageBox.Show(card.Description, "Wylosowana karta", MessageBoxButton.OK);
        }


        private void HandlePlanetInteraction()
        {
            if (_hasPromptedThisTurn) return;

            var currentPlayer = _game.State.CurrentPlayer;
            var currentField = _game.State.GalaxyMap.Fields[currentPlayer.Position];

            if (currentField.Type == FieldType.Planet && currentField.Planet != null)
            {
                var planet = currentField.Planet;

                if (!planet.IsOwned)
                {
                    var result = MessageBox.Show($"{planet.Name} kosztuje {planet.Price} kredytów. Chcesz kupić?", "Zakup planety", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        if (planet.Buy(currentPlayer))
                        {
                            MessageBox.Show($"Zakupiono planetę {planet.Name}!", "Sukces");
                        }
                        else
                        {
                            MessageBox.Show("Nie masz wystarczającej liczby kredytów lub planeta jest już zajęta.", "Błąd");
                        }
                    }
                }
            }

            _hasPromptedThisTurn = true;

            UpdateTurnUI();
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
        }
    }
}
