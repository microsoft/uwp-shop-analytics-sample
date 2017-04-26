using System;
using System.Linq;
using RidoShop.Client.Helpers;
using System.Collections.ObjectModel;
using ShopEvents.Models;

namespace RidoShop.Client.ViewModels
{
    public class InsightsViewModel : Observable
    {
        public ObservableCollection<WeeklyData> ByDayOfWeek { get; private set; } = new ObservableCollection<WeeklyData>();

        public ObservableCollection<HourlyData> ByHour { get; private set; } = new ObservableCollection<HourlyData>();

        public InsightsViewModel()
        {

        }

        public void LoadData()
        {
            var all = MainViewModel.Current.ShopEvents;

            var resWeekly = all.GroupBy(e => e.EventTime.DayOfWeek, 
                            (k, v) => new WeeklyData(k.ToString(), v.Count()))
                        .OrderByDescending(k=>k.WeekValue);

            foreach (var item in resWeekly)
            {
                ByDayOfWeek.Add(new WeeklyData(item.Week, item.WeekValue));
            }

            var resHour = all.GroupBy(e => e.EventTime.Hour,
                            (k, v) => new HourlyData(k.ToString(), v.Count()))
                        .OrderByDescending(k => k.HourValue);

            foreach (var item in resHour)
            {
                ByHour.Add(new HourlyData(item.Hour, item.HourValue));
            }
        }

    }
}
