using Newtonsoft.Json;
using System.Collections.Generic;

namespace UWPShop.Model
{
    public class HourStats
    {
        public int Hour { get; set; }
        public int Total { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static IEnumerable<HourStats> FromJson(string json)
        {
            return JsonConvert.DeserializeObject<IEnumerable<HourStats>>(json);
        }
    }
}
