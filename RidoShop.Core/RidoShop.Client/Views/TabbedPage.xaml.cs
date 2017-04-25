using RidoShop.Client.ViewModels;

using Windows.UI.Xaml.Controls;

namespace RidoShop.Client.Views
{
    public sealed partial class TabbedPage : Page
    {
        public TabbedViewModel ViewModel { get; } = new TabbedViewModel();
        public TabbedPage()
        {
            InitializeComponent();
        }
    }
}
