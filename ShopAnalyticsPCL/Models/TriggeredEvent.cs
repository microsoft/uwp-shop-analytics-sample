using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ShopAnalyticsPCL.Models
{
    public class TriggeredEvent
    {
        /// <summary>
        /// Represents the event type for the IoT Device - true means someone has entered; false means someone has exited
        /// </summary>
        public bool EventType { get; set; }

        /// <summary>
        /// Represents the exact moment at which an event is triggered
        /// </summary>
        public DateTime EventTime { get; set; }


        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static IList<TriggeredEvent> FromJson(string content)
        {
            return JsonConvert.DeserializeObject<IList<TriggeredEvent>>(content);
        }
    }
}