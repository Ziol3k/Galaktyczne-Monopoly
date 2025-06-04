using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace GalacticMonopoly.UI.Views
{
    public partial class CardDrawWindow : Window
    {
        private List<string> _cardImages;
        private string _finalImagePath;

        public CardDrawWindow(List<string> cardImagePaths, string finalImagePath)
        {
            InitializeComponent();
            _cardImages = cardImagePaths;
            _finalImagePath = finalImagePath;
            this.Loaded += CardDrawWindow_Loaded;
        }

        private async void CardDrawWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await PlayAnimation();
            await Task.Delay(700); // chwilka na wybranej karcie
            this.Close();
        }

        private async Task PlayAnimation()
        {
            var rand = new Random();
            int count = _cardImages.Count;
            for (int i = 0; i < 24; i++)
            {
                string path = _cardImages[rand.Next(count)];
                CardImage.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                FadeIn();
                await Task.Delay(80 + i * 10);
            }
            // Ostateczna karta
            CardImage.Source = new BitmapImage(new Uri(_finalImagePath, UriKind.Relative));
            FadeIn();
            await Task.Delay(2000); // <-- PRZYTRZYMANIE końcowego widoku karty na 2 sekundy
        }


        private void FadeIn()
        {
            CardImage.Opacity = 0;
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(100));
            CardImage.BeginAnimation(OpacityProperty, fadeIn);
        }
    }
}
