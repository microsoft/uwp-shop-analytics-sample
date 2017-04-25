using RidoShop.Client.ViewModels;
using ShopEvents.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace RidoShop.Client.Views
{
    public sealed partial class MasterDetailDetailPage : Page
    {
        public MasterDetailDetailViewModel ViewModel { get; } = new MasterDetailDetailViewModel();
        public MasterDetailDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Item = e.Parameter as TriggeredEvent;
        }
    }
}
