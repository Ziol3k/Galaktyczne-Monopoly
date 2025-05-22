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
    }
}
