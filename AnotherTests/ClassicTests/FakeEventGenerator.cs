using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.Web.Http;
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
        private Windows.Web.Http.HttpClient client;
        private readonly string baseuri = Keys.AzureWebAppUri;

        [TestMethod]
        public async Task SendAnotherEvent()
        {
            var newEvent = new TriggeredEvent
            {
                EventType = true,
                EventTime = DateTime.Now
            };

            //client = new Windows.Web.Http.HttpClient { BaseAddress = new Uri(baseuri) };
            
            HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(newEvent), System.Text.Encoding.UTF8, "application/json");
            //Windows.Web.Http.HttpResponseMessage message = await client.PostAsync("api/event", contentPost);
            Assert.IsNotNull(message);
            Debug.WriteLine(message);
        }
    }
}
