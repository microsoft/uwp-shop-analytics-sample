using System;
using Windows.ApplicationModel.Core;
using Windows.Networking.PushNotifications;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DataClient.ViewModels;
using Microsoft.WindowsAzure.Messaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DataClient.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly NotificationHub hub;
        private readonly MainPageViewModel mpvm;
        private PushNotificationChannel channel;

        public MainPage()
        {
            InitializeComponent();
            hub = new NotificationHub(ShopAnalyticsPCL.Resources.Keys.NhNamespaceName,
                ShopAnalyticsPCL.Resources.Keys.NhListenConnection);
            mpvm = new MainPageViewModel();
            // Changes the UI based on first load
            mpvm.IsFirstLoad = true;
            DataContext = mpvm;
            Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await mpvm.RefreshTheData();
            channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            channel.PushNotificationReceived += Channel_PushNotificationReceived;
            await hub.RegisterNativeAsync(channel.Uri);
            mpvm.IsFirstLoad = false;
            this.DataContext = mpvm;
        }

        private async void Channel_PushNotificationReceived(PushNotificationChannel sender,
            PushNotificationReceivedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.High,
                async () =>
                {
                    // Resetting DataContext to workaround UI update issue with XAML UI Toolkit 
                    await mpvm.RefreshTheData();
                    this.DataContext = mpvm;
                }
                );
        }
    }
}