using System.Windows;
using GalacticMonopoly.Core.Game;
using GalacticMonopoly.UI.Views;

namespace GalacticMonopoly.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // na start pokazujemy menu
            MainFrame.Navigate(new MainMenuPage(this));
        }

        public void StartGame(Game game)
        {
            MainFrame.Navigate(new GameBoardPage(game));
        }
    }
}
