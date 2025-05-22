// GalacticMonopoly.UI/Views/AvatarSelectionWindow.xaml.cs
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GalacticMonopoly.UI.Views
{
    public partial class AvatarSelectionWindow : Window
    {
        /// <summary>
        /// Pełna, absolutna ścieżka do wybranego avatara.
        /// </summary>
        public string SelectedAvatarPathAbsolute { get; private set; }

        private Border _selectedBorder;

        public AvatarSelectionWindow()
        {
            InitializeComponent();

            var folderOnDisk = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Images", "avatars");

            if (!Directory.Exists(folderOnDisk))
            {
                MessageBox.Show($"Nie znaleziono folderu: {folderOnDisk}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (var filePath in Directory.GetFiles(folderOnDisk, "*.png"))
            {
                var image = new Image
                {
                    Source = new BitmapImage(new Uri(filePath, UriKind.Absolute)),
                    Width = 64,
                    Height = 64,
                    Margin = new Thickness(5)
                };

                var border = new Border
                {
                    BorderThickness = new Thickness(2),
                    BorderBrush = Brushes.Transparent,
                    Child = image,
                    Margin = new Thickness(5)
                };

                border.MouseDown += (_, __) =>
                {
                    if (_selectedBorder != null)
                        _selectedBorder.BorderBrush = Brushes.Transparent;

                    _selectedBorder = border;
                    border.BorderBrush = Brushes.LimeGreen;
                    SelectedAvatarPathAbsolute = filePath;
                };

                AvatarPanel.Children.Add(border);
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SelectedAvatarPathAbsolute))
                DialogResult = true;
            else
                MessageBox.Show("Wybierz najpierw avatar.", "Brak wyboru", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
