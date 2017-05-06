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
using RidoShop.Server.Services;


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
        public  async Task<IEnumerable<ShopSensorEvent>> Get(int daysSince = 30)
        {
            return await DocDBRepository<ShopSensorEvent>
                .GetItemsAsync(s => s.EventTime > DateTime.Now.AddDays(daysSince * -1));
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody]ShopSensorEvent newEvent)
        {
            await DocDBRepository<ShopSensorEvent>.CreateItemAsync(newEvent);

            var pushNotificationResponse = await PushNotificationService.TriggerPushNotification(newEvent.EventType, 
                                                    config.Notifications.HubName, config.Notifications.FullListener);

            HttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.OK);
            return res;
        }

        
        [HttpGet("ByDay")]
        public  async Task<IEnumerable<DayStats>> GetEventsByDayOfWeek()
        {
            var all = await Get(30);

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
            var all = await Get(30);
            
            var res = all.GroupBy
                (
                    e => e.EventTime.ToLocalTime().Hour,
                    (k, v) => new { hour = k, num = v.Count() }
                );

            var hours = new[] { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };

            var res2 = from h in hours
                       join r in res on h equals r.hour into ps
                       from p in ps.DefaultIfEmpty()
                       select new HourStats{ Hour = h, Total = p == null ? 0 : p.num };

            return res2;
        }
    }
}
