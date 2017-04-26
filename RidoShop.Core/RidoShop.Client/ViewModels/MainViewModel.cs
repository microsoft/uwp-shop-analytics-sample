using System;

using RidoShop.Client.Helpers;
using RidoShop.Client.BackendServices;
using System.Collections.Generic;
using ShopEvents.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RidoShop.Client.ViewModels
{
    public class MainViewModel : Observable
    {
        public static MainViewModel Current { get; private set; }

        public MainViewModel()
        {
            Current = this;
        }

        public ObservableCollection<TriggeredEvent> ShopEvents { get; private set; } = new ObservableCollection<TriggeredEvent>();

        private int _numEvents;
        public int NumEvents
        {
            get { return _numEvents; }
            set { Set(ref _numEvents,value); }
        }

        internal async Task Initialize()
        {
            ShopEvents.Clear();
            var all = await ShopEventsProxy.GetAllEvents();
            foreach (var e in all)
            {
                ShopEvents.Add(e);
            }
            NumEvents = ShopEvents.Count;
        }
    }
}
