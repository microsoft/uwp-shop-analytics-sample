using RidoShop.Client.ViewModels;

using Windows.UI.Xaml.Controls;

namespace RidoShop.Client.Views
{
    public sealed partial class InsightsPage : Page
    {
        public InsightsViewModel ViewModel { get; } = new InsightsViewModel();
        public InsightsPage()
        {
            InitializeComponent();
        }
    }
}
