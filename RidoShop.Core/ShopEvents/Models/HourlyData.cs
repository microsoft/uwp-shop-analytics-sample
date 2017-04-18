using System.ComponentModel;

namespace ShopEvents.Models
{
    public class HourlyData : BaseModel
    {
        private int hourValue;

        /// <summary>
        /// Used to visualize the number of people that entered the store at each business hour during the business day yesterday
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="value"></param>
        public HourlyData(string hour, int value)
        {
            Hour = hour;
            HourValue = value;
        }

        /// <summary>
        /// Represents each business hour for the store (10 AM - 10 PM)
        /// </summary>
        public string Hour { get; set; }

        /// <summary>
        /// Represents the total number of people entering the store for each business hour
        /// </summary>
        public int HourValue
        {
            get { return hourValue; }
            set
            {
                hourValue = value;
                OnPropertyChanged("HourValue");
            }
        }
    }
}