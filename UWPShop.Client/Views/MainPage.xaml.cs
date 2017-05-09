using UWPShop.Client.ViewModels;
using UWPShop.Model;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
namespace UWPShop.Client.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();
        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.Initialize();
        }

    }
}
