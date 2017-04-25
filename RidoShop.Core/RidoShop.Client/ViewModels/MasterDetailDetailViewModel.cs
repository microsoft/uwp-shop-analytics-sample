using System;
using System.Windows.Input;

using RidoShop.Client.Helpers;
using RidoShop.Client.Services;

using Windows.UI.Xaml;
using ShopEvents.Models;

namespace RidoShop.Client.ViewModels
{
    public class MasterDetailDetailViewModel : Observable
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        public ICommand StateChangedCommand { get; private set; }

        private TriggeredEvent _item;
        public TriggeredEvent Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public MasterDetailDetailViewModel()
        {
            StateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
        }
        
        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            if (args.OldState.Name == NarrowStateName && args.NewState.Name == WideStateName)
            {
                NavigationService.GoBack();
            }
        }
    }
}
