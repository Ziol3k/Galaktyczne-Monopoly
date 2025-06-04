using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Controls
{
    public partial class FieldControl : UserControl
    {
        public FieldControl()
        {
            InitializeComponent();
        }

        public string FieldName
        {
            get => (string)GetValue(FieldNameProperty);
            set => SetValue(FieldNameProperty, value);
        }
        public static readonly DependencyProperty FieldNameProperty =
            DependencyProperty.Register(nameof(FieldName), typeof(string), typeof(FieldControl), new PropertyMetadata(""));

        public string BottomInfo
        {
            get => (string)GetValue(BottomInfoProperty);
            set => SetValue(BottomInfoProperty, value);
        }
        public static readonly DependencyProperty BottomInfoProperty =
            DependencyProperty.Register(nameof(BottomInfo), typeof(string), typeof(FieldControl), new PropertyMetadata(""));

        public ImageSource IconSource
        {
            get => (ImageSource)GetValue(IconSourceProperty);
            set => SetValue(IconSourceProperty, value);
        }
        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.Register(nameof(IconSource), typeof(ImageSource), typeof(FieldControl),
                new PropertyMetadata(null, OnIconSourceChanged));

        private static void OnIconSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FieldControl control && control.Icon != null)
            {
                control.Icon.Source = (ImageSource)e.NewValue;
            }
        }

        public Brush BackgroundColor
        {
            get => (Brush)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register(nameof(BackgroundColor), typeof(Brush), typeof(FieldControl), new PropertyMetadata(Brushes.Black));

        public List<UIElement> Occupants
        {
            get => (List<UIElement>)GetValue(OccupantsProperty);
            set => SetValue(OccupantsProperty, value);
        }
        public static readonly DependencyProperty OccupantsProperty =
            DependencyProperty.Register(nameof(Occupants), typeof(List<UIElement>), typeof(FieldControl),
                new PropertyMetadata(new List<UIElement>(), OnOccupantsChanged));

        private static void OnOccupantsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FieldControl control && control.OccupantsPanel != null)
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
            DependencyProperty.Register(nameof(TopMarkers), typeof(List<UIElement>), typeof(FieldControl),
                new PropertyMetadata(new List<UIElement>(), OnTopMarkersChanged));

        private static void OnTopMarkersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FieldControl control && control.TopMarkerPanel != null)
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
