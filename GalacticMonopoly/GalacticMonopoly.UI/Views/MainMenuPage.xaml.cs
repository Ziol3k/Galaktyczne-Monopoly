using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GalacticMonopoly.Core.Game;
using GalacticMonopoly.Core.Models;
using Controls;
using GalacticMonopoly.Core.Factories; // <-- poprawny using!

namespace GalacticMonopoly.UI.Views
{
    public partial class MainMenuPage : Page
    {
        private readonly MainWindow _mw;
        private readonly List<PlayerSetupControl> _players = new();

        public MainMenuPage(MainWindow mw)
        {
            InitializeComponent();
            _mw = mw;
        }

        private void PlayerCount_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button b) || !int.TryParse(b.Tag.ToString(), out int cnt))
                return;

            PlayersPanel.Children.Clear();
            _players.Clear();

            for (int i = 1; i <= cnt; i++)
            {
                var ctrl = new PlayerSetupControl(i)
                {
                    AvatarSelector = ShowAvatarDialog
                };
                _players.Add(ctrl);
                PlayersPanel.Children.Add(ctrl);
            }
        }

        private string ShowAvatarDialog(int playerNumber)
        {
            var dlg = new AvatarSelectionWindow();
            return (dlg.ShowDialog() == true)
                ? dlg.SelectedAvatarPathAbsolute
                : "";
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            if (_players.Count < 2)
            {
                MessageBox.Show("Wybierz co najmniej 2 graczy.");
                return;
            }

            var game = new Game();
            var map = DefaultMapFactory.Create(); // <-- używaj tylko tej fabryki!
            game.InitializeGame(_players.Count, map);

            for (int i = 0; i < _players.Count; i++)
            {
                var corePlayer = game.State.Players[i];
                var uiCtrl = _players[i];

                corePlayer.Name = uiCtrl.PlayerName;
                corePlayer.AvatarPath = uiCtrl.SelectedAvatarPathAbsolute;
            }

            _mw.StartGame(game);
        }
    }
}
