using RidoShop.Client.ViewModels;

using Windows.UI.Xaml.Controls;

namespace RidoShop.Client.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();
        public MainPage()
        {
            InitializeComponent();
        }
    }
}
