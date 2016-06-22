using System.ComponentModel;

namespace ShopAnalyticsPCL.Models
{
    public class WeeklyData : BaseModel
    {
        private int weekValue;

        /// <summary>
        /// Used to visualize the total number of customers entering the store for each of the past n weeks
        /// </summary>
        /// <param name="week"></param>
        /// <param name="value"></param>
        public WeeklyData(string week, int value)
        {
            Week = week;
            weekValue = value;
        }


        /// <summary>
        /// Represents the week, beginning on Sunday
        /// </summary>
        public string Week { get; set; }


        /// <summary>
        /// Represents the number of cusomters entering the store per week
        /// </summary>
        public int WeekValue
        {
            get { return weekValue; }
            set
            {
                weekValue = value;
                OnPropertyChanged("WeekValue");
            }
        }
    }
}