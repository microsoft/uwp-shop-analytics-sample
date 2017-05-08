using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace UWPShop.Model
{
    public class DayStats
    {
        public DayOfWeek Day { get; set; }
        public int Total { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static IEnumerable<DayStats> FromJson(string json)
        {
            return JsonConvert.DeserializeObject<IEnumerable<DayStats>>(json);
        }
    }
}
