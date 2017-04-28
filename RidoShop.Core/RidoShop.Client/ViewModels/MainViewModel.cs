using System;
using RidoShop.Client.Helpers;
using RidoShop.Client.BackendServices;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RidoShop.Model;
using Windows.UI.Xaml;
using Humanizer;
using System.Linq;

namespace RidoShop.Client.ViewModels
{
    public class MainViewModel : Observable
    {
        private DispatcherTimer timer;
        private DateTime _last = DateTime.Now;


        public static MainViewModel Current { get; private set; }

        public MainViewModel()
        {
            Current = this;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            TimeIdle = (DateTime.Now - _last).Humanize(2);
        }

        public ObservableCollection<ShopSensorEvent> ShopEvents { get; private set; } = new ObservableCollection<ShopSensorEvent>();

        private int _numEvents;
        public int NumEvents
        {
            get { return _numEvents; }
            set { Set(ref _numEvents,value); }
        }

        private string _timeIdle;

        public string TimeIdle
        {
            get { return _timeIdle; }
            set { Set(ref _timeIdle,value); }
        }

        private int _peopleInStore;

        public int PeopleInStore
        {
            get { return _peopleInStore; }
            set { Set(ref _peopleInStore,value); }
        }


        internal async Task Initialize()
        {
            
            var todayEvents = await ShopEventsProxy.GetTodayEvents();
            _last = todayEvents.OrderByDescending(e => e.EventTime).FirstOrDefault().EventTime;

            var enters = todayEvents.Where(e => e.EventType == true).Count();
            var exits = todayEvents.Where(e => e.EventType == false).Count();
            PeopleInStore = enters - exits;

            //ShopEvents.Clear();
            //foreach (var e in await ShopEventsProxy.GetAllEvents())
            //{
            //    ShopEvents.Add(e);
            //}

        }
    }
}
