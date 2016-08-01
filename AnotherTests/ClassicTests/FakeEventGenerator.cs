using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAnalyticsPCL.Resources;
using System.Threading.Tasks;
using ShopAnalyticsPCL.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Diagnostics;

namespace ClassicTests
{
    [TestClass]
    public class FakeEventGenerator
    {
        private HttpClient client;
        private readonly string baseuri = Keys.AzureWebAppUri;

        [TestMethod]
        public async Task SendAnotherEvent()
        {
            var newEvent = new TriggeredEvent
            {
                EventType = true,
                EventTime = DateTime.Now
            };

            client = new HttpClient { BaseAddress = new Uri(baseuri) };
            
            HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(newEvent), System.Text.Encoding.UTF8, "application/json");

            var message = await client.PostAsync("api/event", contentPost);
            Assert.IsNotNull(message);
            Debug.WriteLine(message);
        }
    }
}
