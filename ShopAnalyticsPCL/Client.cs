using ShopAnalyticsPCL.Models;
using ShopAnalyticsPCL.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShopAnalyticsPCL
{
    public class Client
    {
        HttpClient http = new HttpClient() { BaseAddress = new Uri(Keys.AzureWebAppUri)};

        public async Task CreateEvent(bool eventType)
        {
            var newEvent = new TriggeredEvent
            {
                EventType = eventType,
                EventTime = DateTime.Now
            };

            var json = newEvent.ToJson();
            
            HttpResponseMessage message = await http
                .PostAsync("api/event", 
                    new StringContent(json,
                            Encoding.UTF8,"application/json"));
        }

        public async Task<IList<TriggeredEvent>> ReadAllEvents()
        {
            var response = await http.GetAsync("api/event");
            var content = await response.Content.ReadAsStringAsync();
            //return TriggeredEvent.FromJson(content);
            return null;
        }
    }
}
