using System;

namespace RidoShop.Model
{
    public partial class ShopSensorEvent
    {
        public string id { get; set; }
        public bool EventType { get; set; }
        
        public DateTime EventTime { get; set; }
    }
    public partial class DayStats
    {
        public DayOfWeek Day { get; set; }
        public int Total { get; set; }
    }
    public partial class HourStats
    {
        public int Hour { get; set; }
        public int Total { get; set; }
    }
}
