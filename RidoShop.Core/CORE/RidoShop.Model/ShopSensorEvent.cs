using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace RidoShop.Model
{
    public partial class ShopSensorEvent
    {
        public string id { get; set; }
        public bool EventType { get; set; }
        public DateTime EventTime { get; set; }
    }
    public class DayStats
    {
        public DayOfWeek Day { get; set; }
        public int Total { get; set; }
    }
    public class HourStats
    {
        public int Hour { get; set; }
        public int Total { get; set; }
    }
}
