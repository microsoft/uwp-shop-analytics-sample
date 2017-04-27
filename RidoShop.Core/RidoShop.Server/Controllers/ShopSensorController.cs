using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RidoShop.Model;
using System.Net.Http;
using Microsoft.Azure.NotificationHubs;
using System.Net;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RidoShop.Server.Controllers
{
    [Route("api/[controller]")]
    public class ShopSensorController : Controller
    {
        private readonly AppConfig config;
        public ShopSensorController(IOptions<AppConfig> optionsAccesor)
        {
            config = optionsAccesor.Value;
            DocDBRepository<ShopSensorEvent>.Initialize(config.DocDb.Uri, config.DocDb.Key);            
        }

        [HttpGet]
        public  async Task<IEnumerable<ShopSensorEvent>> Get(int daysSince = 2)
        {
            return await DocDBRepository<ShopSensorEvent>
                .GetItemsAsync(s => s.EventTime > DateTime.Now.AddDays(daysSince * -1));
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(ShopSensorEvent newEvent)
        {
            await DocDBRepository<ShopSensorEvent>.CreateItemAsync(newEvent);

            var pushNotificationResponse = await TriggerPushNotification(newEvent.EventType);

            HttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.OK);
            return res;
        }

        [HttpGet("total")]
        public async Task<int> GetTotal(int daysSince = 2)
        {
            return await DocDBRepository<ShopSensorEvent>
                .GetNumEvents(s => s.EventTime > DateTime.Now.AddDays(daysSince*-1));
        }

        [HttpGet("ByDay")]
        public  async Task<IEnumerable<DayStats>> GetEventsByDayOfWeek()
        {
            var all = await Get();

            var res = all.GroupBy
                (
                    e => e.EventTime.DayOfWeek,
                    (k, v) => new { day = k, num = v.Count() }
                );

            var res2 = from d in Enum.GetValues(typeof(DayOfWeek)).OfType<DayOfWeek>().ToList()
                       join r in res on d equals r.day into ps
                       from p in ps.DefaultIfEmpty()
                       select new DayStats{ Day= d, Total = p == null ? 0 : p.num };

            return res2;
        }

        [HttpGet("ByHour")]
        public async Task<IEnumerable<HourStats>> GetEventsByHourOfDay()
        {
            var all = await Get();

            var res = all.GroupBy
                (
                    e => e.EventTime.Hour,
                    (k, v) => new { hour = k, num = v.Count() }
                );

            var hours = new[] { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };

            var res2 = from h in hours
                       join r in res on h equals r.hour into ps
                       from p in ps.DefaultIfEmpty()
                       select new HourStats{ Hour = h, Total = p == null ? 0 : p.num };

            return res2;
        }


        private async Task<NotificationOutcome> TriggerPushNotification(bool eventType)
        {
            string windowsToastPayload;
            // Get the Notification Hubs credentials for the Mobile App.
            // Create the notification hub client.
            var hub = NotificationHubClient
                .CreateClientFromConnectionString(config.Notifications.FullListener, config.Notifications.HubName);

            // Define a WNS payload
            if (eventType == true)
            {
                windowsToastPayload = @"<toast><visual><binding template=""ToastText01""><text id=""1"">"
                                      + "Someone has entered the store" + @"</text></binding></visual></toast>";
            }
            else
            {
                windowsToastPayload = @"<toast><visual><binding template=""ToastText01""><text id=""1"">"
                                      + "Someone has exited the store" + @"</text></binding></visual></toast>";
            }

            return await hub.SendWindowsNativeNotificationAsync(windowsToastPayload);
        }


        

       
    }
}
