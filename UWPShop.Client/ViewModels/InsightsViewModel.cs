using System;
using System.Linq;
using UWPShop.Client.Helpers;
using System.Collections.ObjectModel;
using UWPShop.Model;
using UWPShop.Client.BackendServices;

namespace UWPShop.Client.ViewModels
{
    public class InsightsViewModel : Observable
    {
        public ObservableCollection<DayStats> ByDayOfWeek { get; private set; } = new ObservableCollection<DayStats>();

        public ObservableCollection<HourStats> ByHour { get; private set; } = new ObservableCollection<HourStats>();

        public InsightsViewModel()
        {

        }

        public async void LoadData()
        {

            var resWeekly = await ShopEventsProxy.GetWeeklyData();

            foreach (var item in resWeekly)
            {                
                ByDayOfWeek.Add(item);
            }

            var resHour = await ShopEventsProxy.GetHourlyData();

            foreach (var item in resHour)
            {
                ByHour.Add(item);
            }
        }

    }
}
