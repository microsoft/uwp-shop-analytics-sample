
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ShopEvents.Models;
using ShopEvents;

namespace RidoShop.xTests
{
    public class Client
    {
        HttpClient http = new HttpClient() { BaseAddress = new Uri("http://ridoshop.azurewebsites.net") };

        public async Task CreateEvent(bool eventType)
        {
            var newEvent = new TriggeredEvent
            {
                EventType = eventType,
                EventTime = DateTime.Now.AddDays(-1)
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
          
            return Serializer<IList<TriggeredEvent>>.FromJson(content);
        }
    }
}
