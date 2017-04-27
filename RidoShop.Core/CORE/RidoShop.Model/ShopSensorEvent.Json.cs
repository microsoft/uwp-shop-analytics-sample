using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using System.Text;

namespace RidoShop.Model
{

    public partial class ShopSensorEvent
    {
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static IEnumerable<ShopSensorEvent> FromJson(string json)
        {
            return JsonConvert.DeserializeObject<IEnumerable<ShopSensorEvent>>(json);
        }
    }


    public partial class DayStats
    {
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static IEnumerable<DayStats> FromJson(string json)
        {
            return JsonConvert.DeserializeObject<IEnumerable<DayStats>>(json);
        }
    }


    public partial class HourStats
    {
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
