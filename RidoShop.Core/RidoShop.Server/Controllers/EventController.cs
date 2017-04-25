using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RidoShop.Server;
using ShopEvents.Models;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Options;

namespace EventApi.Controllers
{
    [Route("api/[controller]")]
    public class EventController : Controller
    {

        private readonly AppConfig config;
        private readonly TriggeredEventRepository repository;

        public EventController(IOptions<AppConfig> optionsAccesor)
        {
            config = optionsAccesor.Value;
            repository = new TriggeredEventRepository(new Uri(config.DocDb.Uri),config.DocDb.Key);
        }


        // URI for docDB service


        // GET: api/Event/5
        /// <summary>
        ///     Returns the list of events as a list of TriggeredEvent objects
        /// </summary>
        /// <returns></returns>
        public async Task<IList<TriggeredEvent>> Get()
        {
            return await repository.GetAll();   
        }


        // POST: api/Event
        /// <summary>
        ///     Adds newEvent to the DocDB and sends a push notification to the client app
        /// </summary>
        /// <param name="newEvent"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> Post(TriggeredEvent newEvent)
        {
            await repository.Add(newEvent);
            
            var pushNotificationResponse = await TriggerPushNotification(newEvent.EventType);

            HttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.OK);
            return res;
            //return Request.CreateResponse(HttpStatusCode.OK, newEvent);
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