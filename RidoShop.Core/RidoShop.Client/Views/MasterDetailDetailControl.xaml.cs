using ShopEvents.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace RidoShop.Client.Views
{
    public sealed partial class MasterDetailDetailControl : UserControl
    {
        public TriggeredEvent MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as TriggeredEvent; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem",typeof(TriggeredEvent),typeof(MasterDetailDetailControl),new PropertyMetadata(null));

        public MasterDetailDetailControl()
        {
            InitializeComponent();
        }
    }
}
