using System;
using System.Linq;
using RidoShop.Client.Helpers;
using System.Collections.ObjectModel;
using RidoShop.Model;
using RidoShop.Client.BackendServices;

namespace RidoShop.Client.ViewModels
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
