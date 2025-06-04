using System;
using System.Windows;
using GalacticMonopoly.Core.Enums;
using GalacticMonopoly.Core.Game;
using GalacticMonopoly.Core.Models;
using GalacticMonopoly.Core.Rules;

namespace GalacticMonopoly.UI.Views
{
    public partial class PlayerAssetsWindow : Window
    {
        private readonly PlayerAssetsViewModel _viewModel;
        private readonly Player _player;
        private readonly GameState _gameState;
        private readonly bool _isCurrentPlayer;
        private readonly Action _onUpgrade;

        public PlayerAssetsWindow(Player player, GameState state, bool isCurrentPlayer, bool hasUpgradedThisTurn, Action onUpgrade = null)
        {
            InitializeComponent();

            _viewModel = new PlayerAssetsViewModel(player, state, isCurrentPlayer, hasUpgradedThisTurn);
            DataContext = _viewModel;

            _player = player;
            _gameState = state;
            _isCurrentPlayer = isCurrentPlayer;
            _onUpgrade = onUpgrade;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpgradeToFarm_Click(object sender, RoutedEventArgs e)
        {
            UpgradeSpacePortWithType(sender, StructureType.Farm);
        }
        private void UpgradeToMine_Click(object sender, RoutedEventArgs e)
        {
            UpgradeSpacePortWithType(sender, StructureType.Mine);
        }
        private void UpgradeToOutpost_Click(object sender, RoutedEventArgs e)
        {
            UpgradeSpacePortWithType(sender, StructureType.Outpost);
        }

        private void UpgradeSpacePortWithType(object sender, StructureType targetType)
        {
            if (!_isCurrentPlayer)
            {
                MessageBox.Show("Możesz rozbudowywać tylko w swojej turze!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var btn = sender as FrameworkElement;
            var planetView = btn?.Tag as PlanetView;
            if (planetView?.PlanetRef == null) return;

            if (_player.HasUpgradedThisTurn)
            {
                MessageBox.Show("Możesz ulepszyć lub zbudować tylko jedną budowlę w tej turze!", "Limit", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                UpgradeRules.UpgradeSpacePortTo(planetView.PlanetRef.structure, targetType, _player);
                _player.HasUpgradedThisTurn = true;
                MessageBox.Show("Rozbudowano port kosmiczny!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                _onUpgrade?.Invoke();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Upgrade_Click(object sender, RoutedEventArgs e)
        {
            if (!_isCurrentPlayer)
            {
                MessageBox.Show("Możesz ulepszać tylko w swojej turze!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var btn = sender as FrameworkElement;
            var planetView = btn?.Tag as PlanetView;
            if (planetView?.PlanetRef == null) return;

            if (_player.HasUpgradedThisTurn)
            {
                MessageBox.Show("Możesz ulepszyć lub zbudować tylko jedną budowlę w tej turze!", "Limit", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var owningSystem = _gameState.GalaxyMap.Systems.Find(s => s.Planets.Contains(planetView.PlanetRef));
                UpgradeRules.Upgrade(planetView.PlanetRef.structure, _player, owningSystem);
                _player.HasUpgradedThisTurn = true;
                MessageBox.Show("Budowla została ulepszona!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                _onUpgrade?.Invoke();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Nie można ulepszyć", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BuildShipyard_Click(object sender, RoutedEventArgs e)
        {
            if (!_isCurrentPlayer)
            {
                MessageBox.Show("Możesz budować tylko w swojej turze!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var btn = sender as FrameworkElement;
            var sysView = btn?.Tag as PlanetSystemView;
            if (sysView == null || sysView.SystemRef == null) return;

            try
            {
                UpgradeRules.BuildSystemStructure(
                    sysView.SystemRef, StructureType.GalacticShipyard, _player
                );
                _player.HasUpgradedThisTurn = true;
                MessageBox.Show("Zbudowano stocznię galaktyczną!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                _onUpgrade?.Invoke();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BuildAsteroidMine_Click(object sender, RoutedEventArgs e)
        {
            if (!_isCurrentPlayer)
            {
                MessageBox.Show("Możesz budować tylko w swojej turze!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var btn = sender as FrameworkElement;
            var sysView = btn?.Tag as PlanetSystemView;
            if (sysView == null || sysView.SystemRef == null) return;

            try
            {
                UpgradeRules.BuildSystemStructure(
                    sysView.SystemRef, StructureType.AsteroidBeltMine, _player
                );
                _player.HasUpgradedThisTurn = true;
                MessageBox.Show("Zbudowano kopalnię w pasie asteroid!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                _onUpgrade?.Invoke();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
