
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UWPShop.Model;

namespace UWPShop.Tests
{
    class Client
    {

        HttpClient http = new HttpClient() { BaseAddress = new Uri("http://localhost:58032") };
        

        public async Task CreateEvent(bool eventType)
        {
            var newEvent = new ShopSensorEvent
            {
                EventType = eventType,
                EventTime = DateTime.Now.AddDays(-2)
            };

            var json = newEvent.ToJson();

            HttpResponseMessage message = await http
                .PostAsync("api/ShopSensor",
                    new StringContent(json,
                            Encoding.UTF8, "application/json"));
        }

        public async Task<IEnumerable<ShopSensorEvent>> ReadAllEvents()
        {
            var response = await http.GetAsync("api/ShopSensor?DaysSince=100");
            var content = await response.Content.ReadAsStringAsync();
            return ShopSensorEvent.FromJson(content);
        }

        public async Task<IEnumerable<ShopSensorEvent>> ReadTodayEvents()
        {            
            return ShopSensorEvent.FromJson(
                    await (await http.GetAsync("api/ShopSensor?DaysSince=2"))
                    .Content.ReadAsStringAsync());
        }
    }
}
