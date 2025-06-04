using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GalacticMonopoly.UI.Views
{
    public partial class GalacticTicketWindow : Window
    {
        public int? SelectedStationIndex { get; private set; }

        private readonly List<int> _stationFieldIndices;

        public GalacticTicketWindow(List<(int index, string name)> stations)
        {
            InitializeComponent();
            _stationFieldIndices = stations.Select(s => s.index).ToList();
            StationsList.ItemsSource = stations.Select(x => $"[{x.index}] {x.name}").ToList();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (StationsList.SelectedIndex >= 0)
            {
                SelectedStationIndex = _stationFieldIndices[StationsList.SelectedIndex];
                DialogResult = true;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            SelectedStationIndex = null;
            DialogResult = false;
        }
    }
}
