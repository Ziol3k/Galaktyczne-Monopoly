using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        public object InnerContent
        {
            get => GetValue(InnerContentProperty);
            set => SetValue(InnerContentProperty, value);
        }

        public static readonly DependencyProperty InnerContentProperty =
            DependencyProperty.Register(nameof(InnerContent), typeof(object), typeof(PlanetFieldControl), new PropertyMetadata(null));
    }
}
