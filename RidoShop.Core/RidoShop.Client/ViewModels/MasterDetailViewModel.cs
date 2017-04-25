using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using RidoShop.Client.Helpers;
using RidoShop.Client.Services;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ShopEvents.Models;

namespace RidoShop.Client.ViewModels
{
    public class MasterDetailViewModel : Observable
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        private VisualState _currentState;

        private TriggeredEvent _selected;
        public TriggeredEvent Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        private string _totalItems;
        public string TotalItems {
            get { return _totalItems; }
            set { Set(ref _totalItems, value); }
         }

        public ICommand ItemClickCommand { get; private set; }
        public ICommand StateChangedCommand { get; private set; }

        public ObservableCollection<TriggeredEvent> SampleItems { get; private set; } = new ObservableCollection<TriggeredEvent>();

        public MasterDetailViewModel()
        {
            ItemClickCommand = new RelayCommand<ItemClickEventArgs>(OnItemClick);
            StateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
        }

        public async Task LoadDataAsync(VisualState currentState)
        {
            _currentState = currentState;
            SampleItems.Clear();

            var service = new SampleModelService();
            var data = await service.GetDataAsync();

            foreach (var item in data)
            {
                SampleItems.Add(item);
            }
            TotalItems = SampleItems.Count().ToString();
            Selected = SampleItems.First();
        }

        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            _currentState = args.NewState;
        }

        private void OnItemClick(ItemClickEventArgs args)
        {
            TriggeredEvent item = args?.ClickedItem as TriggeredEvent;
            if (item != null)
            {
                if (_currentState.Name == NarrowStateName)
                {
                    NavigationService.Navigate<Views.MasterDetailDetailPage>(item);
                }
                else
                {
                    Selected = item;
                }
            }
        }
    }
}
