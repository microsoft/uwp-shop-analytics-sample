using RidoShop.Client.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
namespace RidoShop.Client.Views
{
    public sealed partial class InsightsPage : Page
    {
        public InsightsViewModel ViewModel { get; } = new InsightsViewModel();
        public InsightsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.LoadData();
        }
    }
}
