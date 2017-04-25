using System;
using System.Linq;
using RidoShop.Client.Helpers;
using System.Collections.ObjectModel;
using ShopEvents.Models;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using RidoShop.Client.Services;

namespace RidoShop.Client.ViewModels
{
    public class MainViewModel : Observable
    {
        private VisualState _currentState;
        public string _numItems;
        public ObservableCollection<TriggeredEvent> SampleItems { get; private set; } = new ObservableCollection<TriggeredEvent>();

        public string NumItems 
        {
            get{ return _numItems; }
            set{ Set(ref _numItems, value);}
        }


        public MainViewModel()
        {
        }

        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            _currentState = args.NewState;
        }

        public async Task LoadDataAsync(VisualState currentState)
        {
            _currentState = currentState;
            SampleItems.Clear();
            var service = new SampleModelService();
            var data = await service.GetDataAsync();
            //NumItems = data.ToList().Count.ToString() + " items";
        }
    }
}
