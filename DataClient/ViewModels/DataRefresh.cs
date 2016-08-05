using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShopAnalyticsPCL.Models;

namespace DataClient.ViewModels
{
    public class DataRefresh
    {
        /// <summary>
        ///     Gets rawdata from the web API Get Method
        /// </summary>
        /// <returns></returns>
        public static async Task<ObservableCollection<TriggeredEvent>> RefreshRawData()
        {
            var client = new HttpClient {BaseAddress = new Uri(ShopAnalyticsPCL.Resources.Keys.AzureWebAppUri)};
            var response = await client.GetAsync("api/event");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ObservableCollection<TriggeredEvent>>(content);
        }

        /// <summary>
        ///     Determines how many people are in the store currently
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static int CountNumPeopleInStore(ObservableCollection<TriggeredEvent> rawData)
        {
            var numPeopleInStore = 0;
            foreach (var e in rawData)
            {
                if (e.EventType)
                {
                    numPeopleInStore++;
                }
                else
                {
                    numPeopleInStore--;
                }
            }
            return numPeopleInStore;
        }

        /// <summary>
        ///     Converts rawData into a more enumerated value for easier operations
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static ObservableCollection<TransformedData> TransformRawData(
            ObservableCollection<TriggeredEvent> rawData)
        {
            var transformedDataCollection = new ObservableCollection<TransformedData>();
            foreach (var e in rawData)
            {
                var transformedData = new TransformedData();
                transformedData.Day = e.EventTime.Day;
                transformedData.Minute = e.EventTime.Minute;
                transformedData.Hour = e.EventTime.Hour;
                transformedData.Year = e.EventTime.Year;
                transformedData.Month = e.EventTime.Month;
                if (e.EventType)
                {
                    transformedData.Value = 1;
                }
                else
                {
                    transformedData.Value = -1;
                }
                transformedDataCollection.Add(transformedData);
            }
            return transformedDataCollection;
        }

        /// <summary>
        ///     Finds the number of customers that entered the shop yesterday between the hours of 10 am and 10 pm (shop hours)
        /// </summary>
        /// <param name="transformedDataCollection"></param>
        /// <returns></returns>
        public static ObservableCollection<HourlyData> TransformHourlyData(
            ObservableCollection<TransformedData> transformedDataCollection)
        {
            var hourlyInsights = new ObservableCollection<HourlyData>();
            // Only show data for business hours - 10 AM - 10 PM
            for (var i = 10; i <= 22; i++)
            {
                if (i < 12)
                {
                    hourlyInsights.Add(new HourlyData(i + " AM", 0));
                }
                else if (i == 12)
                {
                    hourlyInsights.Add(new HourlyData(i + " PM", 0));
                }
                else
                {
                    hourlyInsights.Add(new HourlyData((i - 12) + " PM", 0));
                }
            }

            foreach (var data in transformedDataCollection)
            {
                if (IsYesterday(data))
                {
                    if (data.Hour >= 10 && data.Hour <= 22)
                    {
                        var data1 = hourlyInsights.ToArray();
                        if (data1 == null) throw new ArgumentNullException(nameof(data1));
                        if (data.Value == 1)
                        {
                            hourlyInsights[data.Hour - 10].HourValue += data.Value;
                        }
                    }
                }
            }
            return hourlyInsights;
        }


        /// <summary>
        ///     Over the last 5 weeks, determines average number of customers per day of week, as well as total number of customers
        ///     for each of the weeks
        /// </summary>
        /// <param name="weeklyInsights"></param>
        /// <param name="averageDailyInsights"></param>
        /// <param name="rawData"></param>
        public static void TransformWeeklyData(ObservableCollection<WeeklyData> weeklyInsights,
            ObservableCollection<AverageDailyData> averageDailyInsights, ObservableCollection<TriggeredEvent> rawData)
        {
            // Start the week on Sunday - when GetHashCode == 0
            var sunday = DateTime.Now.AddDays(DayOfWeekConverter(DateTime.Now.DayOfWeek)*-1);


            for (var i = 4; i >= 0; i--)
            {
                var firstDayOfWeek = sunday.AddDays(i*-7);
                weeklyInsights.Add(new WeeklyData(firstDayOfWeek.Month + "/" + firstDayOfWeek.Day, 0));
            }
            for (var j = 0; j < 7; j++)
            {
                var day = sunday.AddDays(j);
                averageDailyInsights.Add(new AverageDailyData(day.DayOfWeek.ToString(), 0));
            }
            foreach (var e in rawData)
            {
                if (e.EventTime.CompareTo(sunday.AddDays(-7*4)) < 0)
                {
                }
                else
                {
                    if (e.EventType)
                    {
                        AddAverageDailyData(e, averageDailyInsights);
                        if (e.EventTime.CompareTo(sunday.AddDays(-7*3)) < 0)
                        {
                            weeklyInsights[0].WeekValue++;
                        }
                        else if (e.EventTime.CompareTo(sunday.AddDays(-7*2)) < 0)
                        {
                            weeklyInsights[1].WeekValue++;
                        }
                        else if (e.EventTime.CompareTo(sunday.AddDays(-7*1)) < 0)
                        {
                            weeklyInsights[2].WeekValue++;
                        }
                        else if (e.EventTime.CompareTo(sunday) < 0)
                        {
                            weeklyInsights[3].WeekValue++;
                        }
                        else
                        {
                            weeklyInsights[4].WeekValue++;
                        }
                    }
                }
            }
            foreach (var data in averageDailyInsights)
            {
                // turn this into average number of customers per day (over 5 weeks)
                data.AverageDayValue /= 5;
            }
        }


        private static void AddAverageDailyData(TriggeredEvent e,
            ObservableCollection<AverageDailyData> averageDailyInsights)
        {
            averageDailyInsights[DayOfWeekConverter(e.EventTime.DayOfWeek)].AverageDayValue += 1;
        }

        private static int DayOfWeekConverter(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Sunday:
                    return 0;
                case DayOfWeek.Monday:
                    return 1;
                case DayOfWeek.Tuesday:
                    return 2;
                case DayOfWeek.Wednesday:
                    return 3;
                case DayOfWeek.Thursday:
                    return 4;
                case DayOfWeek.Friday:
                    return 5;
                case DayOfWeek.Saturday:
                    return 6;
                default:
                    return -1;
            }
        }

        private static bool IsYesterday(TransformedData data)
        {
            DateTime yesterday;
            yesterday = DateTime.Now.AddDays(-1);
            if (data.Day == yesterday.Day && data.Month == yesterday.Month &&
                data.Year == yesterday.Year)
            {
                return true;
            }
            return false;
        }
    }
}