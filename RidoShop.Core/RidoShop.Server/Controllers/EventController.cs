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
    [Route("api/[controller]")]
    public class EventController : Controller
    {
        // URI for docDB service
        private readonly static string endpointUri = "https://ridoshopdb.documents.azure.com:443/";// Keys.DocDbUri;

        // Defines the exact pointer to the document used for this program
        private readonly string databaseName = "";//  Keys.DocDbName;
        private readonly string collectionName = "";// Keys.DocDbCollectionName;
        private readonly string documentName = "";// Keys.DocDbDocName;

        private readonly string docDbKey = "";//Keys.DocDbKey;

        private readonly TriggeredEventRepository repository = new TriggeredEventRepository(
            new Uri(endpointUri),
            "20ySChTRW4mVpQEb5aP3Oy3Nxvxiph0HH0EoE6hDA0vRG7XPuwcPaIclOQX5Gmh15afj7nTcNxmAlAD0mxTOzw==");

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
            string notificationHubName = "eventshub";//Keys.NhName;
            string notificationHubConnection = "Endpoint=sb://photoseventsnh.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=DniAXgy7XXHaTesc7/jul4Hp0xF6dy8u0p5dKjFwAso=";//Keys.NhFullConnection;

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