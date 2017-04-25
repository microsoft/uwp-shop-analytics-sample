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

namespace EventApi.Controllers
{
    public class EventController : Controller
    {
        // URI for docDB service
        private readonly string endpointUri = "";// Keys.DocDbUri;

        // Defines the exact pointer to the document used for this program
        private readonly string databaseName = "";//  Keys.DocDbName;
        private readonly string collectionName = "";// Keys.DocDbCollectionName;
        private readonly string documentName = "";// Keys.DocDbDocName;

        private readonly string docDbKey = "";//Keys.DocDbKey;

        private readonly TriggeredEventRepository repository = new TriggeredEventRepository(
            new Uri(""), 
            "");

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
            string notificationHubName = "";//Keys.NhName;
            string notificationHubConnection = "";//Keys.NhFullConnection;

            // Create the notification hub client.
            var hub = NotificationHubClient
                .CreateClientFromConnectionString(notificationHubConnection, notificationHubName);

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