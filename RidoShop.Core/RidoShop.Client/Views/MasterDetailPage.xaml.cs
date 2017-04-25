using RidoShop.Client.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace RidoShop.Client.Views
{
    public sealed partial class MasterDetailPage : Page
    {
        public MasterDetailViewModel ViewModel { get; } = new MasterDetailViewModel();
        public MasterDetailPage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadDataAsync(WindowStates.CurrentState);
        }
    }
}
