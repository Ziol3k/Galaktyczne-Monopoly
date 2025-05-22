// Controls/PlayerSetupControl.xaml.cs
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Controls
{
    public partial class PlayerSetupControl : UserControl
    {
        public int PlayerNumber { get; }

        /// <summary>Wpisana nazwa gracza.</summary>
        public string PlayerName => NameBox.Text;

        /// <summary>Absolutna ścieżka do wybranego avatara.</summary>
        public string SelectedAvatarPathAbsolute { get; private set; } = "";

        /// <summary>
        /// Delegat, który otwiera dialog i zwraca absolutną ścieżkę do avatara.
        /// </summary>
        public Func<int, string> AvatarSelector { get; set; }

        public PlayerSetupControl(int number)
        {
            InitializeComponent();
            PlayerNumber = number;
        }

        private void SelectAvatar_Click(object sender, RoutedEventArgs e)
        {
            if (AvatarSelector == null) return;

            // Otwórz dialog i pobierz absolutną ścieżkę
            var path = AvatarSelector(PlayerNumber);
            if (string.IsNullOrEmpty(path)) return;

            SelectedAvatarPathAbsolute = path;

            // Podgląd avatara
            AvatarImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
        }
    }
}
