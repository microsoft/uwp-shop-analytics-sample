using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.NotificationHubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShopAnalyticsPCL.Models;
using ShopAnalyticsPCL.Resources;

namespace EventApi.Controllers
{
    public class EventController : ApiController
    {
        // URI for docDB service
        private readonly string endpointUri = Keys.DocDbUri;

        // Defines the exact pointer to the document used for this program
        private readonly string databaseName = Keys.DocDbName;
        private readonly string collectionName = Keys.DocDbCollectionName;
        private readonly string documentName = Keys.DocDbDocName;

        private readonly string docDbKey = Keys.DocDbKey;

        private DocumentClient documentClient;


        // GET: api/Event/5
        /// <summary>
        ///     Returns the list of events as a list of TriggeredEvent objects
        /// </summary>
        /// <returns></returns>
        public async Task<IList<TriggeredEvent>> Get()
        {
            // Returns the document object response from DocDB - response.Resource is the document contents
            var response = await GetDocResponse();
            var readDocument = response.Resource;
            var documentContents = JObject.Parse(readDocument.ToString());
            IList<JToken> records = documentContents["records"]?.Children().ToList();
            IList<TriggeredEvent> triggeredEvents = new List<TriggeredEvent>();
            if (records != null)
            {
                foreach (var record in records)
                {
                    var triggeredEvent = JsonConvert.DeserializeObject<TriggeredEvent>(record.ToString());
                    triggeredEvents.Add(triggeredEvent);
                }
            }
            //Returns ALL events that have been recorded
            return triggeredEvents;
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
            var collectionLink = UriFactory.CreateDocumentCollectionUri(databaseName, collectionName);
            var response = await GetDocResponse();
            var upserted = response.Resource;
            var events = upserted.GetPropertyValue<List<TriggeredEvent>>("records");
            events.Add(newEvent);
            upserted.SetPropertyValue("records", events);
            response = await documentClient.UpsertDocumentAsync(collectionLink, upserted);
            var pushNotificationResponse = await TriggerPushNotification(newEvent.EventType);

            return Request.CreateResponse(HttpStatusCode.OK, newEvent);
        }

        private async Task<NotificationOutcome> TriggerPushNotification(bool eventType)
        {
            string windowsToastPayload;
            // Get the Notification Hubs credentials for the Mobile App.
            string notificationHubName = Keys.NhNamespaceName;
            string notificationHubConnection = Keys.NhFullConnection;

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

        /// <summary>
        ///     Returns document used as the DB for this app
        /// </summary>
        /// <param name="endpointURI"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task<ResourceResponse<Document>> GetDocResponse()
        {
            documentClient = new DocumentClient(new Uri(endpointUri), docDbKey);
            var documentLink = UriFactory.CreateDocumentUri(databaseName, collectionName, documentName);
            var response = await documentClient.ReadDocumentAsync(documentLink);
            return response;
        }
    }
}