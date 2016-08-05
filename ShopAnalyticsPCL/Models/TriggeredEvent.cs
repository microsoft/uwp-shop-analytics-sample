using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace ShopAnalyticsPCL.Models
{
    public class TriggeredEvent
    {
        public string id { get; set; }

        /// <summary>
        /// Represents the event type for the IoT Device - true means someone has entered; false means someone has exited
        /// </summary>
        /// 
        public bool EventType { get; set; }

        /// <summary>
        /// Represents the exact moment at which an event is triggered
        /// </summary>
        public DateTime EventTime { get; set; }

        

        public string ToJson()
        {
            return Serializer<TriggeredEvent>.ToJson(this);            
        }

        public static TriggeredEvent FromJson(string content)
        {
            return Serializer<TriggeredEvent>.FromJson(content);            
        }
    }
}