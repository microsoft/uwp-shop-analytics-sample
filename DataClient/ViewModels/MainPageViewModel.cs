using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using ShopAnalyticsPCL.Models;

namespace DataClient.ViewModels
{
    public class MainPageViewModel : BaseModel
    {
        private ObservableCollection<AverageDailyData> averageDailyInsights;

        private string dateLastRefreshed;
        private ObservableCollection<HourlyData> hourlyInsights;

        private int numPeopleInStore;
        private ObservableCollection<TriggeredEvent> rawData = new ObservableCollection<TriggeredEvent>();
        private ObservableCollection<TransformedData> transformedDataCollection;
        private ObservableCollection<WeeklyData> weeklyInsights;

        private bool isFirstLoad;

        public MainPageViewModel()
        {
            hourlyInsights = new ObservableCollection<HourlyData>();
        }

        public bool IsFirstLoad
        {
            get { return isFirstLoad; }
            set
            {
                isFirstLoad = value;
                OnPropertyChanged("IsFirstLoad");
            }
        }

        public ObservableCollection<HourlyData> HourlyInsights
        {
            get { return hourlyInsights; }
            set
            {
                hourlyInsights = value;
                OnPropertyChanged("HourlyInsights");
            }
        }

        public ObservableCollection<WeeklyData> WeeklyInsights
        {
            get { return weeklyInsights; }
            set
            {
                weeklyInsights = value;
                OnPropertyChanged("WeeklyInsights");
            }
        }

        public ObservableCollection<AverageDailyData> AverageDailyInsights
        {
            get { return averageDailyInsights; }
            set
            {
                averageDailyInsights = value;
                OnPropertyChanged("AverageDailyInsights");
            }
        }

        public int NumPeopleInStore
        {
            get { return numPeopleInStore; }
            set
            {
                numPeopleInStore = value;
                OnPropertyChanged("NumPeopleInStore");
            }
        }

        public string DateLastRefreshed
        {
            get { return dateLastRefreshed; }
            set
            {
                dateLastRefreshed = value;
                OnPropertyChanged("DateLastRefreshed");
            }
        }


        public async Task RefreshTheData()
        {
            // Gets the raw data to operate on (from the DocDB document)
            rawData = await DataRefresh.RefreshRawData();
            
            // Calculates the current number of people in the store
            NumPeopleInStore = DataRefresh.CountNumPeopleInStore(rawData);
            
            // Transforms the data collection for easier data operations
            transformedDataCollection = DataRefresh.TransformRawData(rawData);

            // Transforms the data to visualize the per hour number of customers entering the store yesterday
            HourlyInsights = DataRefresh.TransformHourlyData(transformedDataCollection);


            // Transforms the data to visualize the total number of customers per week for the last 5 weeks, 
            // and the average number of customer per day of week for the last 5 weeks
            WeeklyInsights = new ObservableCollection<WeeklyData>();
            AverageDailyInsights = new ObservableCollection<AverageDailyData>();
            DataRefresh.TransformWeeklyData(WeeklyInsights, AverageDailyInsights, rawData);

            // Visualizes when the data was last refreshes
            DateLastRefreshed = "Data Last Refreshed On " + DateTime.Now;
        }
    }
}