using System.ComponentModel;

namespace ShopEvents.Models
{
    public class AverageDailyData : BaseModel
    {
        private double averageDayValue;

        /// <summary>
        /// Used to visualize the average number of people entering the store on each day of the week
        /// </summary>
        /// <param name="day"></param>
        /// <param name="value"></param>
        public AverageDailyData(string day, double value)
        {
            Day = day;
            AverageDayValue = value;
        }


        /// <summary>
        /// Represents the day of week (Sunday, Monday, Tuesday...)
        /// </summary>
        public string Day { get; set; }
        
        /// <summary>
        /// Represents the average number of people entering the store on each day of the week
        /// </summary>
        public double AverageDayValue
        {
            get { return averageDayValue; }
            set
            {
                averageDayValue = value;
                OnPropertyChanged("AverageDayValue");
            }
        }
    }
}