using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RidoShop.Model
{
    public class ShopSensorEvent
    {
        public string id { get; set; }
        public bool EventType { get; set; }
        public DateTime EventTime { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static IEnumerable<ShopSensorEvent> FromJson(string json)
        {
            return JsonConvert.DeserializeObject<IEnumerable<ShopSensorEvent>>(json);
        }
    }
}
