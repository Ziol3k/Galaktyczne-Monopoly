using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Controls
{
    public partial class PlanetFieldControl : UserControl
    {
        public PlanetFieldControl()
        {
            InitializeComponent();
        }

        public string PlanetName
        {
            get => (string)GetValue(PlanetNameProperty);
            set => SetValue(PlanetNameProperty, value);
        }

        public static readonly DependencyProperty PlanetNameProperty =
            DependencyProperty.Register(nameof(PlanetName), typeof(string), typeof(PlanetFieldControl), new PropertyMetadata(""));

        public string BottomInfo
        {
            get => (string)GetValue(BottomInfoProperty);
            set => SetValue(BottomInfoProperty, value);
        }

        public static readonly DependencyProperty BottomInfoProperty =
            DependencyProperty.Register(nameof(BottomInfo), typeof(string), typeof(PlanetFieldControl), new PropertyMetadata(""));

        public ImageSource BackgroundImage
        {
            get => (ImageSource)GetValue(BackgroundImageProperty);
            set => SetValue(BackgroundImageProperty, value);
        }

        public static readonly DependencyProperty BackgroundImageProperty =
            DependencyProperty.Register(nameof(BackgroundImage), typeof(ImageSource), typeof(PlanetFieldControl), new PropertyMetadata(null));

        public Brush BackgroundColor
        {
            get => (Brush)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register(nameof(BackgroundColor), typeof(Brush), typeof(PlanetFieldControl), new PropertyMetadata(Brushes.Black));

        public List<UIElement> Occupants
        {
            get => (List<UIElement>)GetValue(OccupantsProperty);
            set => SetValue(OccupantsProperty, value);
        }

        public static readonly DependencyProperty OccupantsProperty =
            DependencyProperty.Register(nameof(Occupants), typeof(List<UIElement>), typeof(PlanetFieldControl), new PropertyMetadata(new List<UIElement>(), OnOccupantsChanged));

        private static void OnOccupantsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlanetFieldControl control && control.OccupantsPanel != null)
            {
                control.OccupantsPanel.Children.Clear();
                foreach (var el in control.Occupants)
                {
                    control.OccupantsPanel.Children.Add(el);
                }
            }
        }

        public List<UIElement> TopMarkers
        {
            get => (List<UIElement>)GetValue(TopMarkersProperty);
            set => SetValue(TopMarkersProperty, value);
        }

        public static readonly DependencyProperty TopMarkersProperty =
            DependencyProperty.Register(nameof(TopMarkers), typeof(List<UIElement>), typeof(PlanetFieldControl), new PropertyMetadata(new List<UIElement>(), OnTopMarkersChanged));

        private static void OnTopMarkersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlanetFieldControl control && control.TopMarkerPanel != null)
            {
                control.TopMarkerPanel.Children.Clear();
                foreach (var el in control.TopMarkers)
                {
                    control.TopMarkerPanel.Children.Add(el);
                }
            }
        }
    }
}
