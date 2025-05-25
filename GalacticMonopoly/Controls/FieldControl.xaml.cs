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
            get => Label.Text;
            set => Label.Text = value;
        }

        public ImageSource IconSource
        {
            get => Icon.Source;
            set => Icon.Source = value;
        }

        public List<UIElement> Occupants
        {
            get => (List<UIElement>)GetValue(OccupantsProperty);
            set => SetValue(OccupantsProperty, value);
        }

        public static readonly DependencyProperty OccupantsProperty =
            DependencyProperty.Register(nameof(Occupants), typeof(List<UIElement>), typeof(FieldControl), new PropertyMetadata(new List<UIElement>(), OnOccupantsChanged));

        private static void OnOccupantsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FieldControl control)
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
            DependencyProperty.Register(nameof(TopMarkers), typeof(List<UIElement>), typeof(FieldControl), new PropertyMetadata(new List<UIElement>(), OnTopMarkersChanged));

        private static void OnTopMarkersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FieldControl control)
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
